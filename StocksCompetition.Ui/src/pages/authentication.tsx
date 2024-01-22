import { MockGraph } from '@components/authentication/mock-graph'
import { AuthenticationInputs } from '@components/authentication/authentication-inputs'

export const Authentication = () => {
    return (
        <div className='authentication-container'>
            <MockGraph />
            <AuthenticationInputs />
        </div>
    )
}