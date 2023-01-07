module.exports = ({ env }) => ({
  transformer: {
    enabled: true,
    config: {
      prefix: '/api/',
      responseTransforms: {
        removeAttributesKey: true,
        removeDataKey: true,
      },
    },
  },
  'users-permissions': {
    enabled: true,
    config: {
      jwt: {
        expiresIn: '15m',
      },
    },
  },
});
