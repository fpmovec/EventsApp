FROM node
WORKDIR /app/frontend
COPY  package.json .
RUN yarn install
COPY . .
EXPOSE 5173
CMD ["yarn", "run", "dev"]