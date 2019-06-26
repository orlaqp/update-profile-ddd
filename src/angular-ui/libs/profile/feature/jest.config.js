module.exports = {
  name: 'profile-feature',
  preset: '../../../jest.config.js',
  coverageDirectory: '../../../coverage/libs/profile/feature',
  snapshotSerializers: [
    'jest-preset-angular/AngularSnapshotSerializer.js',
    'jest-preset-angular/HTMLCommentSerializer.js'
  ]
};
