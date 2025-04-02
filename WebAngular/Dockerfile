# Use official Node.js image
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Copy package files separately for better caching
COPY package.json package-lock.json ./

# Install dependencies
RUN npm cache clean --force && npm ci

# Copy the rest of the application
COPY . .

# Expose Angular development server port
EXPOSE 4378

# Command to run Angular dev server
CMD ["npm", "run", "start"]