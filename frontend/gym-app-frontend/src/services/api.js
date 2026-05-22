    import axios from "axios";

    const api = axios.create({
        baseURL : "https://gymapp-production-27d2.up.railway.app/api",
        headers:{
            'content-type' : 'application/json',
        },

    });

    api.interceptors.request.use(
        (config) => {
            const token = localStorage.getItem('token');
            if (token){
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error) => Promise.reject(error)
    );

    export default api;