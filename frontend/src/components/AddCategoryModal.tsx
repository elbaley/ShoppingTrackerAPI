import { useAuth } from "@/lib/useAuth";
import { IoAdd } from "react-icons/io5";
import { useState } from "react";
import { toast } from "react-toastify";
import { fetcher } from "@/lib/fetcher";
import Modal from "./Modal";

interface AddCategoryModalProps {
  fetchCategories: () => void;
}

const AddCategoryModal = ({ fetchCategories }: AddCategoryModalProps) => {
  const { user } = useAuth();
  const initialState = {
    name: "",
  };
  const [isOpen, setIsOpen] = useState(false);
  const [formState, setFormState] = useState(initialState);

  function closeModal() {
    setIsOpen(false);
  }

  function openModal() {
    setIsOpen(true);
  }

  function addNewCategory() {
    if (formState.name.length > 3) {
      fetcher({
        url: "/Category",
        method: "POST",
        body: formState,
        token: user.token,
        json: false,
      }).then((data) => {
        if (data.statusCode === 201) {
          toast.success("New category saved!");
          setFormState({ name: "" });
          fetchCategories();
        } else {
          toast.error(data.message);
        }

        closeModal();
      });
    } else {
      toast.error("Name should have at least 3 characters!");
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
        Add list
      </button>

      <Modal title="Add new category" closeModal={closeModal} isOpen={isOpen}>
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
              setFormState((oldState) => {
                const newState = { ...oldState, name: e.target.value };
                return newState;
              });
            }}
            value={formState.name}
          />
        </div>
        <button
          onClick={addNewCategory}
          className="hover:bg-opacity-70 p-2 w-full bg-black text-white rounded-lg"
        >
          Add
        </button>
      </Modal>
    </>
  );
};

export default AddCategoryModal;
