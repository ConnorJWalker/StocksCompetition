import { useState } from 'react'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'
import { AuthenticationOptions } from '@components/authentication/authentication-options'
import { LoginForm } from '@components/authentication/forms/login-form'
import { SignupCredentialsForm } from '@components/authentication/forms/signup-credentials-form'
import { DisplayDetailsForm } from '@components/authentication/forms/display-details-form.tsx'
import { ApiKeyForm } from '@components/authentication/forms/api-key-form.tsx'

export const AuthenticationInputs = () => {
    const [currentForm, setCurrentForm] = useState(AuthenticationFormType.AuthenticationInputs)
    
    const changeForm = (formType: AuthenticationFormType) => setCurrentForm(formType)
    
    const forms = {
        [AuthenticationFormType.AuthenticationInputs]: <AuthenticationOptions changeForm={changeForm} />,
        [AuthenticationFormType.Login]: <LoginForm changeForm={changeForm} />,
        [AuthenticationFormType.SignupCredentials]: <SignupCredentialsForm changeForm={changeForm} />,
        [AuthenticationFormType.SignupDisplayDetails]: <DisplayDetailsForm changeForm={changeForm} />,
        [AuthenticationFormType.SignupApiKey]: <ApiKeyForm changeForm={changeForm} />,
    }
    
    return (
        <div className='authentication-inputs-container'>
            <h1>Stocks Competition</h1>
            <h2>It's all downhill from here...</h2>
            { forms[currentForm] }
        </div>
    )
}