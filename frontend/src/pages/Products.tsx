import ProductCard from "@/components/ProductCard";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { useEffect, useState } from "react";

interface ProductsProps {}

interface IUserList {
  id: number;
  name: string;
  startedShopping: boolean;
}

export interface IProduct {
  id: number;
  name: string;
  price: number;
  category: string;
  productImg: string;
  categoryId: number;
}

const Products = ({}: ProductsProps) => {
  const [products, setProducts] = useState<IProduct[]>([]);
  const [userLists, setUserLists] = useState<IUserList[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<IProduct[]>([]);
  const [categoryFilter, setCategoryFilter] = useState<string | "">("");
  const { user } = useAuth();
  useEffect(() => {
    // fetch products
    fetcher({ url: "/Product", method: "GET", token: user.token }).then(
      (products: IProduct[]) => setProducts(products),
    );
    // fetch user lists
    fetcher({ url: "/UserList", method: "GET", token: user.token }).then(
      (response) => {
        const userLists = response.map((list: any) => ({
          id: list.id,
          name: list.name,
          startedShopping: list.startedShopping,
        }));
        setUserLists(userLists);
      },
    );

    //
  }, []);

  // Extract unique categories from the product data
  const categories = Array.from(
    new Set(products.map((product) => product.category)),
  );

  // Function to filter products based on the selected category
  useEffect(() => {
    if (categoryFilter === "") {
      setFilteredProducts(products); // If no category selected, show all products
    } else {
      const filtered = products.filter(
        (product) => product.category === categoryFilter,
      );
      setFilteredProducts(filtered);
    }
  }, [categoryFilter, products]);
  return (
    <main className="px-3 sm:pl-10 ">
      <div className="flex items-center flex-col sm:flex-row justify-between sm:pr-5">
        <h1 className="pb-3">Products</h1>
      </div>

      {/* Category filter input */}
      <select
        className="px-2 py-1 my-2 border rounded-md"
        value={categoryFilter}
        onChange={(e) => setCategoryFilter(e.target.value)}
      >
        <option value="">All Categories</option>
        {categories.map((category) => (
          <option key={category} value={category}>
            {category}
          </option>
        ))}
      </select>

      <div className="flex gap-2 flex-wrap justify-center md:justify-start">
        {filteredProducts.map((product) => (
          <ProductCard
            key={product.id}
            product={product}
            userLists={userLists}
          />
        ))}
      </div>
    </main>
  );
};

export default Products;
