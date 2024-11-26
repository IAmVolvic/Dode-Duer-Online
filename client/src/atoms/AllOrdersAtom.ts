import { atom } from 'jotai';
import { Api, OrderDto } from '@Api.ts';
import {useAtom} from "jotai/index";
import {useEffect} from "react";

export const AllOrdersAtom = atom<OrderDto[]>([]);

export function initOrdersAtom(){
    const [orders, setOrders] = useAtom<OrderDto[]>(AllOrdersAtom);
    useEffect(() => {
        new Api().order.orderGetOrders().then(r => {
            // @ts-ignore
            setOrders(r.data);
        })
    }, []);
}