import {atom} from "jotai/index";
import {useEffect, useState} from "react";
import {useAtom} from "jotai";
import {Api, PriceDto} from "@Api.ts";

export const PriceAtom = atom<PriceDto[]>([]);

export function initPropertiesAtom(){
    const [properties, setProperties] = useAtom<PriceDto[]>(PriceAtom);
    useEffect(() => {
        new Api().price.priceGetPrices().then(r => {
            setProperties(r.data);
        })
    }, []);
}