const { watch, series } = require('gulp');
const browsersync = require('browser-sync').create();
const axios = require('axios');

function browsersyncInit(done) {
  browsersync.init({
    proxy: 'localhost:5000',
    port: 5001
  });
  done();
}

function reload(done) {
  const url = `${browsersync.getOption('urls').get('local')}/documentation/caches`;

  axios.delete(url)
    .then(() => {
      browsersync.reload();
      done();
    });
}

function watchMarkdown() {
  watch('UmbracoDocs/**/*.md', reload);
}

exports.default = series(browsersyncInit, watchMarkdown);
