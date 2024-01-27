export const SignupValidator = {
    email: async (email: string): Promise<string[]> => {
        const errors: string[] = []
        
        if (email.trim().length === 0) {
            return ['Email address is required']
        }
        
        if (email.indexOf('@') === -1) {
            errors.push('Email address is not valid')
        }
        
        // TODO: call api to check it is unique
        
        return errors
    },
    
    password: (password: string): string[] => {
        return password.trim().length < 8 ? ['Password must be at least 8 characters long'] : []
    },
    
    confirmPassword: (password: string, confirmPassword: string): string[] => {
        return password !== confirmPassword ? ['Passwords do not match'] : []
    },
    
    username: async (username: string): Promise<string[]> => {
        const errors: string[] = []
        
        const trimmedLength = username.trim().length
        if (trimmedLength < 2) {
            errors.push('Username must be at least 2 characters long')
        }
        else if (trimmedLength > 32) {
            errors.push('Username must be less than 32 characters long')
        }
        
        if (username.split(' ').length > 1) {
            errors.push('Username can not contain spaces')
        }
        
        // TODO: call api to check it is unique
        
        return errors
    },
    
    displayName: (displayName: string): string[] => {
        const errors: string[] = []
        
        const trimmedLength = displayName.trim().length
        if (trimmedLength < 2) {
            errors.push('Username must be at least 2 characters long')
        }
        else if (trimmedLength > 32) {
            errors.push('Username must be less than 32 characters long')
        }
        
        return errors
    }
}