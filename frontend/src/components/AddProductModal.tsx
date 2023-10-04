import { useAuth } from "@/lib/useAuth";
import { IoAdd } from "react-icons/io5";
import { useState } from "react";
import Modal from "./Modal";
import { fetcher } from "@/lib/fetcher";
import { toast } from "react-toastify";

interface AddProductModalProps {
  categories: any;
}

const AddProductModal = ({ categories }: AddProductModalProps) => {
  const { user } = useAuth();
  const initialState = {
    name: "",
    price: 1,
    categoryId: -1,
    productImg: "",
  };
  const [isOpen, setIsOpen] = useState(false);
  const [selectedCategoryId, setSelectedCategoryId] = useState<number>(
    categories[0]?.id,
  );
  const [formState, setFormState] = useState(initialState);

  function closeModal() {
    setIsOpen(false);
  }

  function openModal() {
    setIsOpen(true);
  }

  function addNewProduct() {
    if (formState.categoryId !== -1) {
      fetcher({
        url: "/Product",
        method: "POST",
        token: user.token,
        json: false,
        body: formState,
      }).then((data) => {
        console.log("data su", data);
        if (data.statusCode === 201) {
          toast.success("Added new product");
        } else {
          toast.error(data.message);
        }
      });
    }
  }

  return (
    <>
      <button
        type="button"
        onClick={openModal}
        className="flex items-center rounded-md bg-black  px-4 py-2 text-sm font-medium text-white hover:bg-opacity-30 focus:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75"
      >
        <IoAdd />
        Add product
      </button>

      <Modal title="Add new product" closeModal={closeModal} isOpen={isOpen}>
        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="name"
          >
            Product Name
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            onChange={(e) => {
              setFormState((oldState) => {
                const newState = { ...oldState, name: e.target.value };
                return newState;
              });
            }}
            value={formState.name}
          />
        </div>

        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="name"
          >
            Price
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            type="number"
            onChange={(e) => {
              setFormState((oldState) => {
                const newState = { ...oldState, price: e.target.valueAsNumber };
                return newState;
              });
            }}
            value={formState.price}
          />
        </div>

        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="name"
          >
            Product Img Url
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            onChange={(e) => {
              setFormState((oldState) => {
                const newState = { ...oldState, productImg: e.target.value };
                return newState;
              });
            }}
            value={formState.productImg}
          />
        </div>

        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="price"
          >
            Category
          </label>
          <select
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            value={selectedCategoryId}
            onChange={(e) => {
              setSelectedCategoryId(parseInt(e.target.value));
              setFormState((oldState) => {
                const newState = {
                  ...oldState,
                  categoryId: parseInt(e.target.value),
                };
                return newState;
              });
            }}
          >
            {categories.map((category: any) => {
              return (
                <option key={`${category.id}-category`} value={category.id}>
                  {category.name}
                </option>
              );
            })}
          </select>
        </div>
        <button
          onClick={addNewProduct}
          className="hover:bg-opacity-70 p-2 w-full bg-black text-white rounded-lg"
        >
          Add
        </button>
      </Modal>
    </>
  );
};

export default AddProductModal;
