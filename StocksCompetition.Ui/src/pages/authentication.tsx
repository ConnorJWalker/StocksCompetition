import { useState } from 'react'
import { MockGraph } from '@components/authentication/mock-graph'
import { AuthenticationInputs } from '@components/authentication/authentication-inputs'
import { AuthenticationFormsContext } from '@context/authentication-forms-context'
import { AuthenticationFormContent, getEmptyNewAuthenticationFormContent } from '@models/authentication/authentication-form-content'

export const Authentication = () => {
    const [formData, setFormData] = useState<AuthenticationFormContent>(getEmptyNewAuthenticationFormContent())
    
    return (
        <div className='authentication-container'>
            <AuthenticationFormsContext.Provider value={{ formData, setFormData }}>
                <MockGraph />
                <AuthenticationInputs />
            </AuthenticationFormsContext.Provider>
        </div>
    )
}