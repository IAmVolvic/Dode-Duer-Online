import { AuthorizedUserResponseDTO } from "@Api";
import { UserCreateDialog } from "@components/dialogs/userCreate";
import { UserEditDialog } from "@components/dialogs/userEdit";
import { useGetAllUsers } from "@hooks/user/useGetAllUsers";
import { useBoolean } from "@hooks/utils/useBoolean";
import { useState } from "react";
import { FiEdit, FiUserPlus } from "react-icons/fi";

export const UserControl = () => {
    const { isLoading, data, refetch } = useGetAllUsers();
    const [isOpenUserCreate,, setTrueUserCreate, setFalseUserCreate] = useBoolean(false);

    const [isOpenUserEdit,, setTrueUserEdit, setFalseUserEdit] = useBoolean(false);
    const [selectedUser, setSelectedUser] = useState({} as AuthorizedUserResponseDTO);
    const handleOpenUserEditDialog = (user: AuthorizedUserResponseDTO) => {
        setSelectedUser(user);
        setTrueUserEdit();
    }

    return (
        <>
            <div className="flex flex-col gap-5">
                <button onClick={setTrueUserCreate} className="flex flex-row justify-center items-center gap-5 bg-primary text-primary-content rounded-xl w-full h-10"> 
                    <FiUserPlus  />
                    <div>New User</div>
                </button>
                
                <table className="w-full">
                    <thead>
                        <tr className="bg-base-300 h-12 hidden lg:table-row">
                            <th className="rounded-l-xl"></th>
                            <th className="text-xs text-start">Username</th>
                            <th className="text-xs text-start">Role</th>
                            <th className="text-xs text-start">Enrolled</th>
                            <th className="text-xs text-start">Status</th>
                            <th className="rounded-r-xl"></th>
                        </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                        {!isLoading && data && 
                            Object.values(data as AuthorizedUserResponseDTO[]).map((value: AuthorizedUserResponseDTO) => {
                            
                                return (
                                    <tr key={value.id} className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                        <td ><img src={`https://api.dicebear.com/9.x/adventurer/svg?seed=${value?.name}`} alt="user" className="bg-base-100/50 w-10 h-10 rounded-full" /></td>
                                        <td >{value.name}</td>
                                        <td >{value.role}</td>
                                        <td >{value.enrolled}</td>
                                        <td >{value.status}</td>
                                        <td >
                                            <button onClick={() => handleOpenUserEditDialog(value)} className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-12 h-7"> 
                                                <FiEdit /> 
                                            </button>
                                        </td>
                                    </tr>
                                );
                            })
                        }
                    </tbody>
                </table>
            </div>

            <UserCreateDialog isOpen={isOpenUserCreate} close={setFalseUserCreate} refresh={refetch} />
            
            <UserEditDialog isOpen={isOpenUserEdit} close={setFalseUserEdit} user={selectedUser} refresh={refetch} />
        </>
    );
}

