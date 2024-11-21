import {Paper} from "@assets/models/Paper.ts";
import {Prodcuts} from "@modules/products/hooks/useGetProducts.ts";

export interface OrderEntry {
    /** @format int32 */
    quantity?: number;
    /** @format int32 */
    productId?: number | null;

    product?: Prodcuts
}