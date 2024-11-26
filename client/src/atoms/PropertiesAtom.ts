import {atom} from "jotai/index";
import {Property} from "@assets/models/Property.ts";
import {useEffect, useState} from "react";
import {useAtom} from "jotai";
import {Api} from "@Api.ts";

export const PropertiesAtom = atom<Property[]>([]);

export function initPropertiesAtom(){
    const [properties, setProperties] = useAtom<Property[]>(PropertiesAtom);
    useEffect(() => {
        new Api().property.propertyGetAllProperties().then(r => {
            // @ts-ignore
            setProperties(r.data);
        })
    }, []);
}