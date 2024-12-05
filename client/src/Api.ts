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

export interface BoardResponseDTO {
  /** @format guid */
  id?: string;
  /** @format guid */
  userid?: string;
  /** @format guid */
  gameid?: string;
  /** @format decimal */
  price?: number;
  /** @format date */
  dateofpurchase?: string;
  /**
   * @maxItems 8
   * @minItems 5
   */
  numbers?: (number | null)[];
}

export interface PlayBoardDTO {
  /**
   * @format guid
   * @minLength 1
   */
  userid: string;
  /** @format date */
  dateofpurchase?: string;
  /**
   * @maxItems 8
   * @minItems 5
   */
  numbers?: number[];
}

export interface GameResponseDTO {
  /** @format guid */
  id?: string;
  /** @format date */
  date?: string;
  status?: GameStatus;
}

export enum GameStatus {
  Active = 0,
  Inactive = 1,
}

export interface PriceDto {
  /** @format decimal */
  price1?: number;
  /** @format decimal */
  numbers?: number;
}

export interface TransactionResponseDTO {
  /** @format guid */
  id?: string;
  /** @format guid */
  userId?: string;
  phoneNumber?: string;
  username?: string;
  transactionNumber?: string;
  transactionStatus?: string;
}

export interface DepositRequestDTO {
  /**
   * @minLength 8
   * @maxLength 15
   */
  transactionNumber: string;
}

export interface BalanceAdjustmentRequestDTO {
  /** @minLength 1 */
  transactionId: string;
  /**
   * @format decimal
   * @min 0
   * @max 10000
   */
  amount: number;
  adjustment: TransactionAdjustment;
  transactionStatusA: TransactionStatusA;
}

export enum TransactionAdjustment {
  Deposit = "Deposit",
  Deduct = "Deduct",
}

export enum TransactionStatusA {
  Pending = "Pending",
  Approved = "Approved",
  Rejected = "Rejected",
}

export interface AuthorizedUserResponseDTO {
  /** @format guid */
  id?: string;
  name?: string;
  email?: string;
  phoneNumber?: string;
  /** @format decimal */
  balance?: number;
  role?: string;
  enrolled?: string;
  status?: string;
}

export interface UserSignupRequestDTO {
  /** @minLength 1 */
  name: string;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  /**
   * @format phone
   * @minLength 8
   * @maxLength 8
   */
  phoneNumber: string;
}

export interface UserResponseDTO {
  /** @minLength 1 */
  id: string;
  /** @minLength 1 */
  jwt: string;
}

export interface UserLoginRequestDTO {
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  /**
   * @minLength 5
   * @maxLength 32
   */
  password: string;
}

export interface UserEnrollmentRequestDTO {
  /**
   * @minLength 5
   * @maxLength 32
   */
  password: string;
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
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "http://localhost:5001" });
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
 * @baseUrl http://localhost:5001
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  board = {
    /**
     * No description
     *
     * @tags Board
     * @name BoardPlayBoard
     * @request POST:/Board/Play
     */
    boardPlayBoard: (data: PlayBoardDTO, params: RequestParams = {}) =>
      this.request<BoardResponseDTO, any>({
        path: `/Board/Play`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardGetAllBoards
     * @request GET:/Board/GetBoards
     */
    boardGetAllBoards: (params: RequestParams = {}) =>
      this.request<BoardResponseDTO[], any>({
        path: `/Board/GetBoards`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
  game = {
    /**
     * No description
     *
     * @tags Game
     * @name GameNewGame
     * @request POST:/Game/NewGame
     */
    gameNewGame: (data: number, params: RequestParams = {}) =>
      this.request<GameResponseDTO, any>({
        path: `/Game/NewGame`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  price = {
    /**
     * No description
     *
     * @tags Price
     * @name PriceGetPrices
     * @request GET:/Price/GetPrices
     */
    priceGetPrices: (params: RequestParams = {}) =>
      this.request<PriceDto[], any>({
        path: `/Price/GetPrices`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
  transaction = {
    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionPUserDepositReq
     * @request POST:/Transaction/@user/balance/deposit
     */
    transactionPUserDepositReq: (data: DepositRequestDTO, params: RequestParams = {}) =>
      this.request<TransactionResponseDTO, any>({
        path: `/Transaction/@user/balance/deposit`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionPUserTransactionsReqs
     * @request GET:/Transaction/@user/balance/history
     */
    transactionPUserTransactionsReqs: (params: RequestParams = {}) =>
      this.request<TransactionResponseDTO[], any>({
        path: `/Transaction/@user/balance/history`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionPUseBalance
     * @request PATCH:/Transaction/@admin/balance/adjustment
     */
    transactionPUseBalance: (data: BalanceAdjustmentRequestDTO, params: RequestParams = {}) =>
      this.request<boolean, any>({
        path: `/Transaction/@admin/balance/adjustment`,
        method: "PATCH",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionPDepositReqs
     * @request GET:/Transaction/@admin/balance/history
     */
    transactionPDepositReqs: (params: RequestParams = {}) =>
      this.request<TransactionResponseDTO[], any>({
        path: `/Transaction/@admin/balance/history`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
  user = {
    /**
     * No description
     *
     * @tags User
     * @name UserPSignup
     * @request POST:/User/@admin/signup
     */
    userPSignup: (data: UserSignupRequestDTO, params: RequestParams = {}) =>
      this.request<AuthorizedUserResponseDTO, any>({
        path: `/User/@admin/signup`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserPLogin
     * @request POST:/User/@user/login
     */
    userPLogin: (data: UserLoginRequestDTO, params: RequestParams = {}) =>
      this.request<UserResponseDTO, any>({
        path: `/User/@user/login`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserPEnroll
     * @request PATCH:/User/@user/enroll
     */
    userPEnroll: (data: UserEnrollmentRequestDTO, params: RequestParams = {}) =>
      this.request<AuthorizedUserResponseDTO, any>({
        path: `/User/@user/enroll`,
        method: "PATCH",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags User
     * @name UserGGetUser
     * @request GET:/User/@me
     */
    userGGetUser: (params: RequestParams = {}) =>
      this.request<AuthorizedUserResponseDTO, any>({
        path: `/User/@me`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
}
