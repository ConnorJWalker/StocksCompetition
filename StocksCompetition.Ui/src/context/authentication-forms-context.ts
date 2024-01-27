import { createContext, useContext } from 'react'
import { AuthenticationFormContent } from '@models/authentication/authentication-form-content'

interface IAuthenticationFormsContext {
    formData: AuthenticationFormContent | null
    setFormData: (newValue: object) => void
}

export const AuthenticationFormsContext = createContext<IAuthenticationFormsContext>({
    formData: null,
    setFormData: () => {}
})

export const useAuthenticationFormsContext = (): AuthenticationFormContent => {
    const { formData } = useContext(AuthenticationFormsContext)
    
    if (formData === null) {
        throw new Error('useAuthenticationFormsContext must be used with an AuthenticationFormsContext')
    }
    
    return formData
}