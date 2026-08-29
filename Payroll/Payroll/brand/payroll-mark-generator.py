"""Payroll Management brand mark generator.

Source: brand/payroll-mark-source.svg (the user's outlined "group of people" icon, 474x474).
The mark is that icon redrawn as solid silhouettes: three heads and bodies filled with one ink,
the middle figure separated from the outer two by a transparent gap (mask), nothing else.
Outputs into Payroll-HCC/Content/brand/:
  payroll-mark.svg        ink  #111111 on transparent  - logo on white grounds (login card, menus, footer)
  payroll-mark-white.svg  white on transparent          - logo on charcoal grounds (header, login hero)
  logo.svg                = payroll-mark.svg (kept name)
  favicon.svg             ONE white avatar on a charcoal (#242729) rounded tile - browser tab (the 3-figure mark blurs at 16 px)
  favicon-<n>.png 16..512 rasterised with headless Edge, favicon.ico (16/32/48/64) -> also Payroll-HCC/favicon.ico
Run:  python payroll-mark-generator.py
"""
import os, struct, subprocess, tempfile

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.dirname(HERE)
OUT = os.path.join(ROOT, "Payroll-HCC", "Content", "brand")
SITE = os.path.join(ROOT, "Payroll-HCC")
os.makedirs(OUT, exist_ok=True)

INK, WHITE, CHARCOAL = "#111111", "#ffffff", "#242729"

# Geometry (474-unit box of the source icon; outlines converted to outer silhouettes: head r+7, bodies offset +7)
HEAD_L = (113, 160, 43); HEAD_R = (361, 160, 43); HEAD_C = (237, 160, 47)
BODY_L = "M40 314V263c0-22 8-38 24-50 12-9 26-13 40-13h19c17 0 30 5 41 14 8 6 14 15 18 25V314Z"
BODY_R = "M434 314V263c0-22-8-38-24-50-12-9-26-13-40-13h-19c-17 0-30 5-41 14-8 6-14 15-18 25V314Z"
BODY_C = "M137 362V295c0-22 8-39 24-51 12-9 26-13 42-13h17l17 30 17-30h17c16 0 30 4 42 13 16 12 24 29 24 51v67Z"
GAP = 14  # transparent separation between the middle figure and the outer ones
VIEW = "40 117 394 245"   # tight box of the silhouettes (x 40..434, y 117..362)

def mark(fill, mask_id="cut"):
    """Silhouette group. The outer figures are masked by the middle figure dilated by GAP."""
    return (
        f'<defs><mask id="{mask_id}" maskUnits="userSpaceOnUse" x="0" y="0" width="474" height="474">'
        f'<rect width="474" height="474" fill="#fff"/>'
        f'<circle cx="{HEAD_C[0]}" cy="{HEAD_C[1]}" r="{HEAD_C[2] + GAP}" fill="#000"/>'
        f'<path d="{BODY_C}" fill="#000" stroke="#000" stroke-width="{GAP * 2}" stroke-linejoin="round"/>'
        f'</mask></defs>'
        f'<g fill="{fill}">'
        f'<g mask="url(#{mask_id})">'
        f'<circle cx="{HEAD_L[0]}" cy="{HEAD_L[1]}" r="{HEAD_L[2]}"/><path d="{BODY_L}"/>'
        f'<circle cx="{HEAD_R[0]}" cy="{HEAD_R[1]}" r="{HEAD_R[2]}"/><path d="{BODY_R}"/>'
        f'</g>'
        f'<circle cx="{HEAD_C[0]}" cy="{HEAD_C[1]}" r="{HEAD_C[2]}"/><path d="{BODY_C}"/>'
        f'</g>'
    )

def svg(view, body, label="Payroll Management"):
    return (f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="{view}" role="img" aria-label="{label}">'
            f'{body}</svg>\n')

def write(name, content):
    with open(os.path.join(OUT, name), "w", encoding="utf-8", newline="\n") as f:
        f.write(content)
    print("wrote", name)

write("payroll-mark.svg", svg(VIEW, mark(INK)))
write("payroll-mark-white.svg", svg(VIEW, mark(WHITE)))
write("logo.svg", svg(VIEW, mark(INK)))

# favicon: ONE avatar (head + shoulders, the middle figure alone) - white on a charcoal rounded tile.
# A single figure stays legible at 16 px where the three-figure mark turns to noise (user request 2026-08-28).
TILE = 560.0
AV_HEAD = (237, 150, 62)
AV_BODY = "M112 372V318c0-30 12-52 34-66 17-11 37-17 60-17h62c23 0 43 6 60 17 22 14 34 36 34 66v54Z"
aw, ah = 264.0, 284.0            # avatar box: x 105..369, y 88..372
scale = (TILE * 0.60) / ah
tx = (TILE - aw * scale) / 2 - 105 * scale
ty = (TILE - ah * scale) / 2 - 88 * scale
fav = (f'<rect width="{TILE:g}" height="{TILE:g}" rx="{TILE * 0.2:g}" fill="{CHARCOAL}"/>'
       f'<g transform="translate({tx:.2f},{ty:.2f}) scale({scale:.5f})" fill="{WHITE}">'
       f'<circle cx="{AV_HEAD[0]}" cy="{AV_HEAD[1]}" r="{AV_HEAD[2]}"/><path d="{AV_BODY}"/></g>')
write("favicon.svg", svg(f"0 0 {TILE:g} {TILE:g}", fav))

# raster: favicon.svg -> PNGs via headless Edge (transparent corners), then favicon.ico (PNG-in-ICO)
EDGE = [r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"]
edge = next((e for e in EDGE if os.path.exists(e)), None)
if not edge:
    print("Edge not found - PNG/ICO step skipped")
else:
    svg_path = os.path.join(OUT, "favicon.svg").replace(os.sep, "/")
    pngs = {}
    with tempfile.TemporaryDirectory() as td:
        prof = os.path.join(td, "prof")
        for n in (16, 32, 48, 64, 128, 180, 192, 256, 512):
            html = os.path.join(td, f"f{n}.html")
            with open(html, "w", encoding="utf-8") as f:
                f.write(f'<!doctype html><html><head><meta charset="utf-8"><style>html,body{{margin:0;background:transparent}}'
                        f'img{{display:block;width:{n}px;height:{n}px}}</style></head><body>'
                        f'<img src="file:///{svg_path}"></body></html>')
            png = os.path.join(OUT, f"favicon-{n}.png")
            subprocess.run([edge, "--headless=new", "--disable-gpu", "--no-sandbox", "--disable-extensions",
                            f"--user-data-dir={prof}", "--default-background-color=00000000", "--hide-scrollbars",
                            f"--window-size={n},{n}", f"--screenshot={png}", "file:///" + html.replace(os.sep, "/")],
                           capture_output=True, timeout=120)
            if os.path.exists(png):
                pngs[n] = png; print("wrote", os.path.basename(png))
    sizes = [n for n in (16, 32, 48, 64) if n in pngs]
    if sizes:
        entries, data, off = [], b"", 6 + 16 * len(sizes)
        for n in sizes:
            with open(pngs[n], "rb") as f: blob = f.read()
            entries.append(struct.pack("<BBBBHHII", n % 256, n % 256, 0, 0, 1, 32, len(blob), off))
            data += blob; off += len(blob)
        ico = struct.pack("<HHH", 0, 1, len(sizes)) + b"".join(entries) + data
        for dest in (os.path.join(OUT, "favicon.ico"), os.path.join(SITE, "favicon.ico")):
            with open(dest, "wb") as f: f.write(ico)
            print("wrote", os.path.relpath(dest, ROOT))
