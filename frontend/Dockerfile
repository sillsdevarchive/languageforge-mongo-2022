FROM node:alpine

WORKDIR /app

COPY package-lock.json package.json /app/
RUN npm install

COPY tsconfig.json postcss.config.cjs svelte.config.js vite.config.js tailwind.config.cjs /app/
COPY src /app/src
COPY static /app/static


EXPOSE 3000
