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
    }
}