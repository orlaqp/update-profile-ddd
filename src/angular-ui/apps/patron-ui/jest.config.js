module.exports = {
  name: 'patron-ui',
  preset: '../../jest.config.js',
  coverageDirectory: '../../coverage/apps/patron-ui',
  snapshotSerializers: [
    'jest-preset-angular/AngularSnapshotSerializer.js',
    'jest-preset-angular/HTMLCommentSerializer.js'
  ]
};
