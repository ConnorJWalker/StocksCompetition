import { ReactNode, useState } from 'react'
import { AuthenticationFormContent, getEmptyNewAuthenticationFormContent } from '@models/authentication/authentication-form-content'
import { AuthenticationFormsContext } from '@context/authentication-forms-context'

interface props { children: ReactNode }

export const AuthenticationFormsProvider = ({ children }: props) => {
    const [formData, setFormData] = useState<AuthenticationFormContent>(getEmptyNewAuthenticationFormContent())
    
    const updateForm = (newValue: object) => {
        setFormData({ ...formData, ...newValue })
    }
    
    return (
        <AuthenticationFormsContext.Provider value={{ formData, setFormData: updateForm }}>
            { children }
        </AuthenticationFormsContext.Provider>
    )
}