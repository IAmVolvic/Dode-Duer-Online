/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface CustomerDto {
  /** @format int32 */
  id?: number;
  name?: string;
  address?: string;
  phone?: string;
  email?: string;
}

export interface CreateCustomerDto {
  /** @minLength 1 */
  name: string;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
}

export interface GetCustomerDto {
  /**
   * @format email
   * @minLength 1
   */
  email: string;
}

export interface UpdateCustomerDto {
  /**
   * @format int32
   * @min 1
   * @max 2147483647
   */
  id: number;
  name?: string;
  address?: string;
  /** @format phone */
  phone?: string;
  /** @format email */
  email?: string;
}

export interface OrderDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  customerId?: number;
  /** @format double */
  totalAmount?: number;
  orderEntries?: OrderEntryDto[];
  status?: string | null;
}

export interface OrderEntryDto {
  /** @format int32 */
  quantity?: number;
  /** @format int32 */
  productId?: number | null;
}

export interface PaperDto {
  /** @format int32 */
  id?: number;
  name?: string;
  discontinued?: boolean;
  /** @format int32 */
  stock?: number;
  /** @format double */
  price?: number;
  properties?: PropertyDto[];
}

export interface PropertyDto {
  /** @format int32 */
  id?: number;
  propertyName?: string;
}

export interface CreatePaperDto {
  name?: string;
  /** @format int32 */
  stock?: number;
  /** @format double */
  price?: number;
  properties?: PropertyDto[];
}

export interface CreatePropertyDto {
  propertyName?: string;
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "http://localhost:5000" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title My Title
 * @version 1.0.0
 * @baseUrl http://localhost:5000
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  customer = {
    /**
     * No description
     *
     * @tags Customer
     * @name CustomerCreateCustomer
     * @request POST:/Customer/signup
     */
    customerCreateCustomer: (data: CreateCustomerDto, params: RequestParams = {}) =>
      this.request<CustomerDto, any>({
        path: `/Customer/signup`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Customer
     * @name CustomerLoginCustomer
     * @request POST:/Customer/login
     */
    customerLoginCustomer: (data: GetCustomerDto, params: RequestParams = {}) =>
      this.request<CustomerDto, any>({
        path: `/Customer/login`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Customer
     * @name CustomerUpdateCustomer
     * @request PATCH:/Customer/update
     */
    customerUpdateCustomer: (data: UpdateCustomerDto, params: RequestParams = {}) =>
      this.request<CustomerDto, any>({
        path: `/Customer/update`,
        method: "PATCH",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  order = {
    /**
     * No description
     *
     * @tags Order
     * @name OrderCreateOrder
     * @request POST:/Order
     */
    orderCreateOrder: (data: OrderDto, params: RequestParams = {}) =>
      this.request<OrderDto, any>({
        path: `/Order`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Order
     * @name OrderGetOrders
     * @request GET:/Order
     */
    orderGetOrders: (params: RequestParams = {}) =>
      this.request<OrderDto[], any>({
        path: `/Order`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Order
     * @name OrderChangeOrderStatus
     * @request PUT:/Order
     */
    orderChangeOrderStatus: (data: OrderDto, params: RequestParams = {}) =>
      this.request<OrderDto, any>({
        path: `/Order`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  paper = {
    /**
     * No description
     *
     * @tags Paper
     * @name PaperCreatePaper
     * @request POST:/Paper/Create
     */
    paperCreatePaper: (data: CreatePaperDto, params: RequestParams = {}) =>
      this.request<PaperDto, any>({
        path: `/Paper/Create`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Paper
     * @name PaperGetPaper
     * @request GET:/Paper/Get/{id}
     */
    paperGetPaper: (id: number, params: RequestParams = {}) =>
      this.request<PaperDto, any>({
        path: `/Paper/Get/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Paper
     * @name PaperUpdateDiscontinued
     * @request PUT:/Paper/Update/{id}/Discontinued
     */
    paperUpdateDiscontinued: (id: number, data: boolean, params: RequestParams = {}) =>
      this.request<PaperDto, any>({
        path: `/Paper/Update/${id}/Discontinued`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Paper
     * @name PaperUpdateRestock
     * @request PUT:/Paper/Update/{id}/Restock
     */
    paperUpdateRestock: (id: number, data: number, params: RequestParams = {}) =>
      this.request<PaperDto, any>({
        path: `/Paper/Update/${id}/Restock`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  products = {
    /**
     * No description
     *
     * @tags Products
     * @name ProductsGetAllProducts
     * @request GET:/Products
     */
    productsGetAllProducts: (
      query?: {
        search?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<PaperDto[], any>({
        path: `/Products`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),
  };
  property = {
    /**
     * No description
     *
     * @tags Property
     * @name PropertyCreateProperty
     * @request POST:/Property/Create
     */
    propertyCreateProperty: (data: CreatePropertyDto, params: RequestParams = {}) =>
      this.request<PropertyDto, any>({
        path: `/Property/Create`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Property
     * @name PropertyGetAllProperties
     * @request GET:/Property/Get
     */
    propertyGetAllProperties: (params: RequestParams = {}) =>
      this.request<PropertyDto[], any>({
        path: `/Property/Get`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
}
