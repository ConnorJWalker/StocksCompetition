import { MockGraph } from '@components/authentication/mock-graph'
import { AuthenticationInputs } from '@components/authentication/authentication-inputs'
import { ThemeToggle } from '@components/authentication/theme-toggle'
import { AuthenticationFormsProvider } from '@providers/authentication-forms-provider'

export const Authentication = () => {
    return (
        <div className='authentication-container'>
            <AuthenticationFormsProvider>
                <div className='left'>
                    <MockGraph />
                    <ThemeToggle />
                </div>
                <AuthenticationInputs />
            </AuthenticationFormsProvider>
        </div>
    )
}