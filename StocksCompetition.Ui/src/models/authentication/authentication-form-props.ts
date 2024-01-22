import { AuthenticationFormType } from './authentication-form-type'

export default interface AuthenticationFormProps {
    changeForm: (formType: AuthenticationFormType) => void
}