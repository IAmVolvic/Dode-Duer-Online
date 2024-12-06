import toast from "react-hot-toast";

export const ErrorToast = (error) => {
    if (error.response && error.response.data && error.response.data.errors) {
        let errorMessage = '';
            Object.keys(error.response.data.errors).forEach((field) => {
                error.response.data.errors[field].forEach((message) => {
                errorMessage += `${field}: ${message}\n`; 
            });
        });

        toast.error(errorMessage);
      }
}