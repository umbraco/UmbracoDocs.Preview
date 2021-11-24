const { watch, series } = require('gulp');
const browsersync = require('browser-sync').create();

function browsersyncInit(done){
  browsersync.init({
      proxy: 'http://localhost:5000',
      port: 5001
  });
  done();
}

function reload(done) {
  browsersync.reload();
  done();
}

function watchMarkdown() {
  watch('UmbracoDocs/**/*.md', reload);
}

exports.default = series(browsersyncInit, watchMarkdown);
