from pathlib import Path

root = Path(r'C:\Users\Agustin\source\repos\Auth\frontend\src')
replacements = [
    ('@/Common/Components/', '@/components/'),
    ('@/Common/Context/LoadingContext', '@/app/providers'),
    ('@/Common/', '@/components/'),
    ('@/Security/', '@/features/security/'),
    ('@/Documents/', '@/features/documents/'),
    ('@/Partners/', '@/features/partners/'),
    ('@/Helpers/', '@/lib/'),
    ('@/Routes/', '@/app/'),
    ('./Security/', './features/security/'),
    ('./Documents/', './features/documents/'),
    ('./Partners/', './features/partners/'),
    ('./Helpers/', './lib/'),
    ('./Common/', './components/'),
    ('./Routes/', './app/'),
    ('../Security/', '../features/security/'),
    ('../Documents/', '../features/documents/'),
    ('../Partners/', '../features/partners/'),
    ('../Common/', '../components/'),
    ('../Helpers/', '../lib/'),
    ('../Routes/', '../app/'),
    ('../../Security/', '../../features/security/'),
    ('../../Documents/', '../../features/documents/'),
    ('../../Partners/', '../../features/partners/'),
    ('../../Common/', '../../components/'),
    ('../../Helpers/', '../../lib/'),
    ('../../../Security/', '../../../features/security/'),
    ('../../../Documents/', '../../../features/documents/'),
    ('../../../Partners/', '../../../features/partners/'),
    ('../../../Common/', '../../../components/'),
    ('../../../Helpers/', '../../../lib/'),
    ('./Helpers/authHelpers', './lib/auth'),
    ('@/Helpers/authHelpers', '@/lib/auth'),
    ('@/Helpers/executeWithErrorHandling', '@/lib/errorHandling'),
    ('@/Helpers/parseDates', '@/lib/dates'),
    ('@/Helpers/loadScript', '@/lib/loadScript'),
    ('@/Helpers/api', '@/lib/api'),
    ('./Helpers/executeWithErrorHandling', './lib/errorHandling'),
    ('./Helpers/parseDates', './lib/dates'),
    ('./Helpers/loadScript', './lib/loadScript'),
    ('./Helpers/api', './lib/api'),
    ("'./App'", "'./app/App'"),
    ('"./App"', '"./app/App"'),
    ("'./Routes/AppRoutes'", "'./app/routes'"),
    ('"./Routes/AppRoutes"', '"./app/routes"'),
    ("'../Routes/ProtectedRoute'", "'../app/ProtectedRoute'"),
    ('"../Routes/ProtectedRoute"', '"../app/ProtectedRoute"'),
    ("'./Routes/ProtectedRoute'", "'./app/ProtectedRoute'"),
    ('"./Routes/ProtectedRoute"', '"./app/ProtectedRoute"'),
    ("'./Common/Context/LoadingContext'", "'@/app/providers'"),
    ('"./Common/Context/LoadingContext"', '"@/app/providers"'),
    ('"./Helpers/authHelpers"', '"@/lib/auth"'),
    ('"./Helpers/api"', '"@/lib/api"'),
    ('"./Helpers/loadScript"', '"@/lib/loadScript"'),
]

for path in root.rglob('*'):
    if path.is_file() and path.suffix.lower() in {'.ts', '.tsx', '.js', '.jsx'}:
        try:
            text = path.read_text(encoding='utf-8')
        except Exception:
            continue
        updated = text
        for old, new in replacements:
            updated = updated.replace(old, new)
        if updated != text:
            path.write_text(updated, encoding='utf-8')

print('updated')
