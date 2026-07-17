import hceShared from '@halospv3/hce.shared-config/eslintConfig';
import { defineConfig, type Config } from 'eslint/config';

const config: Config[] = defineConfig([
  ...hceShared,
]);
export default config;
