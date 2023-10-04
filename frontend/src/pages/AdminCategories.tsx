import AddCategoryModal from "@/components/AddCategoryModal";
import Modal from "@/components/Modal";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";

interface AdminCategoriesProps {}

interface ICategory {
  id: number;
  name: string;
}

const AdminCategories = ({}: AdminCategoriesProps) => {
  const { user } = useAuth();
  const [categories, setCategories] = useState<ICategory[]>([]);
  function fetchCategories() {
    fetcher({ url: "/Category", method: "GET", token: user.token }).then(
      (categories) => setCategories(categories),
    );
  }
  useEffect(() => {
    fetchCategories();
  }, []);
  return (
    <main className="px-3 sm:pl-10 ">
      <div className="flex items-center flex-col sm:flex-row justify-between sm:pr-5">
        <h1 className="pb-3">Manage Categories</h1>
      </div>
      <AddCategoryModal fetchCategories={fetchCategories} />

      <div className="relative overflow-x-auto">
        <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col"></th>
              <th scope="col" className="py-3">
                Name
              </th>
            </tr>
          </thead>
          <tbody>
            {categories.map((category) => (
              <CategoryItem
                key={`${category.name}-${category.id}`}
                category={category}
                fetchCategories={fetchCategories}
              />
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
};

const CategoryItem = ({
  category,
  fetchCategories,
}: {
  category: ICategory;
  fetchCategories: () => void;
}) => {
  const { user } = useAuth();
  const [isOpen, setIsOpen] = useState(false);
  const [categoryState, setCategoryState] = useState({
    name: category.name,
  });

  function closeModal() {
    setIsOpen(false);
  }

  function openModal() {
    setIsOpen(true);
  }

  function updateCategory() {
    if (categoryState.name !== category.name) {
      fetcher({
        url: `/Category/${category.id}`,
        method: "PUT",
        body: categoryState,
        token: user.token,
        json: false,
      }).then((data) => {
        if (data.statusCode === 200) {
          toast.success("Category updated");
        } else {
          toast.error(data.message);
        }
        closeModal();
        fetchCategories();
      });
    }
  }

  function deleteCategory() {
    fetcher({
      url: `/Category/${category.id}`,
      method: "DELETE",
      token: user.token,
      json: false,
    }).then((data) => {
      if (data.statusCode === 200) {
        toast.success("Category deleted.");
      } else {
        toast.error(data.message);
      }
      closeModal();
    });
  }

  return (
    <>
      <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
        <td className="cursor-pointer" onClick={openModal}>
          EDIT
        </td>
        <th
          scope="row"
          className="py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
        >
          {category.name}
        </th>
      </tr>

      <Modal
        title={`Edit ${category.name}`}
        closeModal={closeModal}
        isOpen={isOpen}
      >
        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="name"
          >
            Category Name
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            onChange={(e) => {
              setCategoryState((oldState) => {
                const newState = { ...oldState, name: e.target.value };
                return newState;
              });
            }}
            value={categoryState.name}
          />
        </div>

        <div className="flex gap-4">
          <button
            onClick={updateCategory}
            className="hover:bg-opacity-70 p-2 w-full bg-black text-white rounded-lg"
          >
            Update
          </button>

          <button
            onClick={deleteCategory}
            className="hover:bg-opacity-70 p-2 w-full bg-red-600 text-white rounded-lg"
          >
            Delete
          </button>
        </div>
      </Modal>
    </>
  );
};

export default AdminCategories;
