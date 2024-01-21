import { MockGraph } from '@components/authentication/mock-graph'
import { AuthenticationOptions } from '@components/authentication/authentication-options'

export const Authentication = () => {
    return (
        <div className='authentication-container'>
            <MockGraph />
            <AuthenticationOptions />
        </div>
    )
}