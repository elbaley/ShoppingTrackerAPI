import { useMemo, useState } from "react";
import {
  MantineReactTable,
  useMantineReactTable,
  type MRT_ColumnDef,
  MRT_TableOptions,
  MRT_Row,
} from "mantine-react-table";
import { IUserProduct } from "@/pages/ListDetail";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { ActionIcon, Flex, Tooltip, Text } from "@mantine/core";
import { ModalsProvider, modals } from "@mantine/modals";
import { IconEdit, IconTrash } from "@tabler/icons-react";
import { toast } from "react-toastify";

interface UserProductsTableProps {
  startedShopping: boolean;
  userProducts: IUserProduct[];
  fetchUserLists: () => void;
}
const UserProductsTable = ({
  startedShopping,
  userProducts,
  fetchUserLists,
}: UserProductsTableProps) => {
  const { user } = useAuth();
  const [tableData, setTableData] = useState<IUserProduct[]>(
    () => userProducts,
  );

  // send delete request to api
  function deleteUserProduct(id: number) {
    fetcher({
      url: `/UserProduct/${id}`,
      method: "DELETE",
      token: user.token,
      json: false,
    }).then((data) => {
      if (data.statusCode !== 200) {
        toast.error(data.message);
      } else {
        toast.success("Successfully deleted!");
        fetchUserLists();
      }
    });
  }

  const handleSaveRow: MRT_TableOptions<IUserProduct>["onEditingRowSave"] =
    async ({ table, row, values }) => {
      const updatedUserProduct = {
        productId: values["product.id"],
        userListId: values.userListId,
        description: values.description,
        isBought: values.status === "Pending" ? false : true,
      };
      // update the user product
      await fetcher({
        url: `/UserProduct/?id=${values.id}`,
        method: "PUT",
        body: updatedUserProduct,
        token: user.token,
      });

      table.setEditingRow(null); //exit editing mode
    };
  //should be memoized or stable
  const columns = useMemo<MRT_ColumnDef<IUserProduct>[]>(
    () => [
      {
        accessorKey: "id", //access nested data with dot notation
        header: "Id",
        enableEditing: false,
      },
      {
        accessorKey: "product.name", //access nested data with dot notation
        header: "Name",
        enableEditing: false,
      },

      {
        accessorKey: "product.price",
        header: "Price",
        enableEditing: false,
      },

      {
        accessorKey: "description",
        header: "Description",
      },
      {
        accessorKey: "status",
        header: "Status",
        editVariant: "select",
        mantineEditSelectProps: {
          data: ["Pending", "Done"],
        },
        Cell: ({ cell }) => {
          const value = cell.getValue() as string;
          return (
            <span
              className={`px-4 py-2 rounded text-white ${
                value === "Done"
                  ? "bg-green-500"
                  : value === "Pending"
                  ? "bg-orange-500"
                  : ""
              }`}
            >
              {value}
            </span>
          );
        },
      },
    ],
    [],
  );

  const openDeleteConfirmModal = (row: MRT_Row<any>) => {
    return modals.openConfirmModal({
      title: "Are you sure you want to delete this user?",
      children: (
        <Text>
          Are you sure you want to delete{" "}
          <strong>{row.original.product.name}</strong>?<br /> This action cannot
          be undone.
        </Text>
      ),
      labels: { confirm: "Delete", cancel: "Cancel" },
      confirmProps: {
        color: "red",
      },
      onConfirm: () => deleteUserProduct(row.original.id),
    });
  };

  const table = useMantineReactTable({
    columns,
    data: userProducts, //must be memoized or stable (useState, useMemo, defined outside of this component, etc.)
    initialState: { density: "xs", columnVisibility: { id: false } },
    enableDensityToggle: false,
    enableEditing: true,
    enableHiding: true,
    onEditingRowSave: handleSaveRow,
    renderRowActions: ({ row, table }) => (
      <Flex gap="md">
        <Tooltip label="Edit">
          <ActionIcon onClick={() => table.setEditingRow(row)}>
            <IconEdit />
          </ActionIcon>
        </Tooltip>
        {startedShopping ? null : (
          <Tooltip label="Delete">
            <ActionIcon color="red" onClick={() => openDeleteConfirmModal(row)}>
              <IconTrash />
            </ActionIcon>
          </Tooltip>
        )}
      </Flex>
    ),
  });
  return (
    <ModalsProvider>
      <MantineReactTable table={table} />
    </ModalsProvider>
  );
};

export default UserProductsTable;
