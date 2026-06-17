import os
import re

def remove_http_client_module(directory):
    count = 0
    # Regular expressions for matching
    import_pattern = re.compile(r"import\s*\{\s*HttpClientModule\s*\}\s*from\s*['\"]@angular/common/http['\"];?\s*\n?")
    # We want to remove HttpClientModule from the imports array. 
    # It could be followed by a comma, or preceded by one.
    usage_pattern1 = re.compile(r"HttpClientModule\s*,?\s*")
    
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.ts') and 'module.ts' in file:
                # We do not want to remove it from app.module.ts or core.module.ts
                if file in ['app.module.ts', 'core.module.ts']:
                    continue
                    
                filepath = os.path.join(root, file)
                try:
                    with open(filepath, 'r', encoding='utf-8') as f:
                        content = f.read()
                        
                    if 'HttpClientModule' in content:
                        # Remove the import statement
                        new_content = import_pattern.sub('', content)
                        
                        # Remove from module imports
                        # This simple regex might leave double commas or trailing commas,
                        # let's try a safer replacement approach for the usage.
                        # We only want to remove `HttpClientModule` as a whole word.
                        
                        # Splitting by HttpClientModule to handle commas correctly
                        # Just removing the word, we can clean up commas if needed
                        new_content = re.sub(r'\bHttpClientModule\b\s*,?', '', new_content)
                        # Clean up any potential empty lines or double commas left behind in imports array
                        new_content = re.sub(r',\s*,', ',', new_content)
                        new_content = re.sub(r'\[\s*,', '[', new_content)
                        new_content = re.sub(r',\s*\]', ']', new_content)

                        if content != new_content:
                            with open(filepath, 'w', encoding='utf-8') as f:
                                f.write(new_content)
                            count += 1
                            print(f"Updated {filepath}")
                except Exception as e:
                    print(f"Failed to process {filepath}: {e}")

    print(f"Total files updated: {count}")

if __name__ == '__main__':
    src_dir = os.path.dirname(os.path.abspath(__file__))
    app_dir = os.path.join(src_dir, 'app')
    remove_http_client_module(app_dir)
