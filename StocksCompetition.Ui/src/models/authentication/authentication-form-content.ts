import { randomColour } from '@utilities/random-colour'

export interface AuthenticationFormContent {
    email: string
    password: string
    confirmPassword: string
    username: string
    displayName: string
    displayColour: string
    profilePicture: string
    apiKey: string
}

export const getEmptyNewAuthenticationFormContent = (): AuthenticationFormContent => ({
    email: '',
    password: '',
    confirmPassword: '',
    username: '',
    displayName: '',
    displayColour: randomColour(),
    profilePicture: '',
    apiKey: ''
})