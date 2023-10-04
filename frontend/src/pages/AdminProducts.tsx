import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { useEffect, useState } from "react";
import { IProduct } from "./Products";
import Modal from "@/components/Modal";
import { toast } from "react-toastify";
import AddProductModal from "@/components/AddProductModal";

interface AdminProductsProps {}

const AdminProducts = ({}: AdminProductsProps) => {
  const [products, setProducts] = useState<IProduct[]>([] as IProduct[]);
  const { user, fetchCategories } = useAuth();
  const [categories, setCategories] = useState([]);

  function fetchProducts() {
    fetcher({ url: "/Product", method: "GET", token: user.token }).then(
      (products) => {
        setProducts(products);
      },
    );
  }

  useEffect(() => {
    fetchProducts();
    fetchCategories().then((categories) => setCategories(categories));
  }, []);

  return (
    <main className="px-3 sm:pl-10 ">
      <div className="flex items-center flex-col sm:flex-row justify-between sm:pr-5">
        <h1 className="pb-3">Manage Products</h1>
      </div>
      <AddProductModal categories={categories} />

      <div className="relative overflow-x-auto">
        <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
          <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
              <th scope="col"></th>
              <th scope="col" className="px-6 py-3">
                Name
              </th>
              <th scope="col" className="px-6 py-3">
                Price
              </th>
              <th scope="col" className="px-6 py-3">
                Category
              </th>
              <th scope="col" className="px-6 py-3">
                IMG Url
              </th>
            </tr>
          </thead>
          <tbody>
            {products.map((product) => (
              <ProductItem
                key={product.id}
                product={product}
                fetchProducts={fetchProducts}
                categories={categories}
              />
            ))}
          </tbody>
        </table>
      </div>
    </main>
  );
};

const ProductItem = ({
  product,
  categories,
  fetchProducts,
}: {
  product: IProduct;
  categories: any;
  fetchProducts: () => void;
}) => {
  const { user } = useAuth();

  const [isOpen, setIsOpen] = useState(false);

  function closeModal() {
    setIsOpen(false);
  }

  function openModal() {
    setIsOpen(true);
  }

  const [productState, setProductState] = useState({
    name: product.name,
    price: product.price,
    productImg: product.productImg,
    categoryId: product.categoryId,
    category: product.category,
  });

  function updateProduct() {
    const updateBody = {
      name: productState.name,
      price: productState.price,
      categoryId: productState.categoryId,
      productImg: productState.productImg,
    };
    fetcher({
      url: `/Product/${product.id}`,
      method: "PUT",
      token: user.token,
      json: false,
      body: updateBody,
    }).then((data: any) => {
      if (data.statusCode === 200) {
        toast.success("Updated the product");
      }
      fetchProducts();
      closeModal();
    });
  }
  function deleteProduct() {
    fetcher({
      url: `/Product/${product.id}`,
      method: "DELETE",
      token: user.token,
      json: false,
    }).then((data) => {
      if (data.statusCode === 200) {
        toast.success("Product deleted");
      } else {
        toast.error(data.message);
      }
      fetchProducts();
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
          className="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
        >
          {product.name}
        </th>
        <td className="px-6 py-4">${product.price}</td>
        <td className="px-6 py-4">{product.category}</td>
        <td className="px-6 py-4">{product.productImg}</td>
      </tr>

      <Modal
        title={`Edit ${product.name}`}
        closeModal={closeModal}
        isOpen={isOpen}
      >
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
              setProductState((oldState) => {
                const newState = { ...oldState, name: e.target.value };
                return newState;
              });
            }}
            value={productState.name}
          />
        </div>

        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="price"
          >
            Price
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            type="number"
            onChange={(e) => {
              setProductState((oldState) => {
                const newState = { ...oldState, price: e.target.valueAsNumber };
                return newState;
              });
            }}
            value={productState.price}
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
            value={productState.categoryId}
            onChange={(e) => {
              setProductState((oldState) => {
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
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              );
            })}
          </select>
        </div>
        <div className="flex gap-4">
          <button
            onClick={updateProduct}
            className="hover:bg-opacity-70 p-2 w-full bg-black text-white rounded-lg"
          >
            Update
          </button>
          <button
            onClick={deleteProduct}
            className="hover:bg-opacity-70 p-2 w-full bg-red-600 text-white rounded-lg"
          >
            Delete
          </button>
        </div>
      </Modal>
    </>
  );
};

export default AdminProducts;
