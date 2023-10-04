import { IoGrid, IoHome, IoPerson, IoSettings, IoCart } from "react-icons/io5";

interface SidebarLink {
  label: string;
  icon: keyof typeof sidebarIcons;
  href: string;
}
export const sidebarIcons = { IoHome, IoGrid, IoPerson, IoSettings, IoCart };

export const sidebarLinks: SidebarLink[] = [
  {
    label: "Products",
    icon: "IoCart",
    href: "/app/products",
  },
  {
    label: "Lists",
    icon: "IoGrid",
    href: "/app/lists",
  },
];
export const adminSidebarLinks: SidebarLink[] = [
  {
    label: "Products",
    icon: "IoCart",
    href: "/app/admin/products",
  },
  {
    label: "Categories",
    icon: "IoGrid",
    href: "/app/admin/categories",
  },
];
