import {PropertyDto} from "@Api.ts";

export interface Paper {
    /** @format int32 */
    id?: number;
    name: string;
    discontinued?: boolean;
    /** @format int32 */
    stock: number;
    /** @format double */
    price: number;
    properties? : PropertyDto[];
}