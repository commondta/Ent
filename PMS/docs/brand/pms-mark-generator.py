"""PMS mark generator — based on the clean vector supplied by the user
(`pms-mark-source.svg`, 810x856).

Run from anywhere:  python docs/brand/pms-mark-generator.py   (needs: pip install shapely pillow)
Writes every SVG into HRMS_Web/wwwroot/img/brand/, then — when Microsoft Edge is installed —
rasterises favicon.svg at native sizes (16…512 PNG, transparent corners) and packs favicon.ico
(16/32/48/64), copying the .ico and the 32px PNG to wwwroot/favicon.ico|png.

Fixes applied to the source:
  * the roof chevron now runs down to a baseline *below* the buildings, so no
    building corner pokes out underneath the roof any more (source roof stopped
    at y=813 while the buildings reached y=817);
  * every building/tower/roof junction has the same clearance (boolean
    geometry, so nothing overlaps or touches);
  * all clearances are parameters -> a heavier-gapped variant for favicon sizes.
Everything is real ink geometry, no background-coloured overlays, so the mark
works on any background.
"""
import os, shutil, subprocess, tempfile
from shapely.geometry import Polygon, box
from shapely.ops import unary_union

HERE = os.path.dirname(os.path.abspath(__file__))
WWWROOT = os.path.normpath(os.path.join(HERE, "..", "..", "HRMS_Web", "wwwroot"))
OUT = os.path.join(WWWROOT, "img", "brand")
os.makedirs(OUT, exist_ok=True)
# Monochrome identity (user rule 2026-08-23): only black, white or gray; background always transparent.
INK = "#111111"
ROOF = "#7E7C7D"

S = 0.5626           # roof slope (rise/run), from the source
APEX = (407.9, 631.3)  # outer roof apex
THICK = 51.5         # roof vertical thickness (682.8 - 631.3)


def build(gap=6.0, roof_gap=14.0, frame_gap=20.0, baseline=840.0, with_frame=True, frame_bottom=868.0,
          window=18.15, win_gap=4.6, win_y=723.19):
    G = 900.0  # "far below" — buildings run down and are cut by the roof clearance

    tower = Polygon([(437.93, 0), (599, 59.23), (599, 174.74), (438, 236), (438, G),
                     (374, G), (374, 401.9), (277, 364.93), (277, 59.14)])
    right = Polygon([(599, 174.0), (762, 236.08), (762, G), (599, G)])
    left = Polygon([(50, 401.92), (211.28, 340.47), (277, 365.6), (277, G), (50, G)])
    tb = tower.buffer(gap, join_style=2)
    right = right.difference(tb)
    left = left.difference(tb)

    ax, ay = APEX
    half_o = (baseline - ay) / S
    roof_outer = Polygon([(ax - half_o, baseline), (ax, ay), (ax + half_o, baseline)])
    half_i = (baseline - (ay + THICK)) / S
    roof_inner = Polygon([(ax - half_i, baseline), (ax, ay + THICK), (ax + half_i, baseline)])
    roof = roof_outer.difference(roof_inner)

    clear = roof_outer.buffer(roof_gap, join_style=2)
    # nothing may sit below the roof baseline either
    clear = unary_union([clear, box(-100, baseline, 1000, G + 10)])
    buildings = unary_union([tower, right, left]).difference(clear)

    wins = []
    x0 = ax - window - win_gap / 2
    for i in range(2):
        for j in range(2):
            x = x0 + i * (window + win_gap)
            y = win_y + j * (window + win_gap)
            wins.append(box(x, y, x + window, y + window))
    windows = unary_union(wins)

    parts = {"ink": buildings, "roof": roof, "windows": windows}
    if with_frame:
        outer = box(0, 103, 810, frame_bottom)
        inner = box(8, 111, 802, frame_bottom - 8)
        tower_top = tower.intersection(box(-10, -10, 900, 300))   # only the part that pierces the rail
        frame = outer.difference(inner).difference(tower_top.buffer(frame_gap, join_style=2))
        parts["frame"] = frame
    return parts


# ------------------------------------------------------------------ svg out
def fmt(v):
    s = f"{v:.2f}".rstrip("0").rstrip(".")
    return "0" if s in ("-0", "") else s


def ring_to_d(coords):
    pts = list(coords)
    if pts[0] == pts[-1]:
        pts = pts[:-1]
    return "M" + " L".join(f"{fmt(x)},{fmt(y)}" for x, y in pts) + "Z"


def geom_to_d(g):
    polys = [g] if g.geom_type == "Polygon" else list(g.geoms)
    d = []
    for p in polys:
        if p.is_empty:
            continue
        d.append(ring_to_d(p.exterior.coords))
        for hole in p.interiors:
            d.append(ring_to_d(hole.coords))
    return " ".join(d)


def path(g, fill, extra="", cls=None):
    c = f' class="{cls}"' if cls else ""
    return f'<path{c} fill-rule="evenodd" fill="{fill}"{extra} d="{geom_to_d(g)}"/>'


def bbox(parts):
    return unary_union(list(parts.values())).bounds


def svg(parts, ink=INK, roof=ROOF, roof_opacity=None, bg=None, viewbox=None, pad=8,
        size=None, tile=None, title="PMS", currentcolor=False):
    if viewbox is None:
        x0, y0, x1, y1 = bbox(parts)
        viewbox = (x0 - pad, y0 - pad, (x1 - x0) + 2 * pad, (y1 - y0) + 2 * pad)
    vb = " ".join(fmt(v) for v in viewbox)
    body = []
    if bg:
        body.append(f'<rect x="{fmt(viewbox[0])}" y="{fmt(viewbox[1])}" width="{fmt(viewbox[2])}" height="{fmt(viewbox[3])}" fill="{bg}"/>')
    if tile:
        body.append(tile)
    if currentcolor:
        ink = "currentColor"
        roof = "currentColor"
        roof_opacity = roof_opacity or "0.72"
    if "frame" in parts:
        body.append(path(parts["frame"], ink))
    body.append(path(parts["ink"], ink))
    ro = f' fill-opacity="{roof_opacity}"' if roof_opacity else ""
    body.append(path(parts["roof"], roof, ro))
    body.append(path(parts["windows"], ink))
    wh = f' width="{size[0]}" height="{size[1]}"' if size else ""
    return (f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="{vb}"{wh} role="img" aria-label="{title}">'
            f'{"".join(body)}</svg>')


def write(name, text):
    with open(os.path.join(OUT, name), "w", encoding="utf-8") as f:
        f.write(text)
    print("wrote", name, len(text), "bytes")


def main():
    T = "Property Management System"
    P = build()
    write("pms-logo.svg", svg(P, title=T))                                   # black + gray, transparent
    write("pms-logo-white.svg", svg(P, ink="#ffffff", roof="#ffffff", roof_opacity="0.72", title=T))  # for dark grounds
    write("pms-logo-current.svg", svg(P, currentcolor=True, title=T))        # follows CSS color
    for stale in ("pms-logo-cream.svg",):                                    # pre-monochrome artefacts
        try: os.remove(os.path.join(OUT, stale))
        except OSError: pass

    # icon / favicon: no frame, heavier clearances, larger windows; bare mark on a
    # transparent square canvas (no tile — the background must stay transparent)
    PI = build(gap=14, roof_gap=26, with_frame=False, window=30, win_gap=9, win_y=712)
    x0, y0, x1, y1 = bbox(PI)
    w, h = x1 - x0, y1 - y0
    side = max(w, h) * 1.04
    cx, cy = (x0 + x1) / 2, (y0 + y1) / 2
    vb = (cx - side / 2, cy - side / 2, side, side)
    write("pms-icon.svg", svg(PI, viewbox=vb, title="PMS"))
    # favicon only: the mark needs a contrasting ground in a browser tab (user rule 2026-08-23) —
    # black rounded tile, white mark; still monochrome. Every other asset stays transparent.
    tside = max(w, h) * 1.18
    tvb = (cx - tside / 2, cy - tside / 2, tside, tside)
    tile = f'<rect x="{fmt(tvb[0])}" y="{fmt(tvb[1])}" width="{fmt(tside)}" height="{fmt(tside)}" rx="{fmt(tside * 0.2)}" fill="{INK}"/>'
    write("favicon.svg", svg(PI, ink="#ffffff", roof="#ffffff", roof_opacity="0.72", viewbox=tvb, tile=tile, title="PMS"))
    write("pms-icon-plain.svg", svg(PI, pad=6, title="PMS"))
    write("pms-icon-plain-white.svg", svg(PI, ink="#ffffff", roof="#ffffff", roof_opacity="0.72", pad=6, title="PMS"))
    write("pms-icon-plain-current.svg", svg(PI, currentcolor=True, pad=6, title="PMS"))

    rasterize()


EDGE = [r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"]


def rasterize(sizes=(16, 32, 48, 64, 180, 192, 512)):
    """favicon.svg -> favicon-<n>.png at native size via headless Edge, then favicon.ico."""
    edge = next((e for e in EDGE if os.path.exists(e)), None)
    if not edge:
        print("Edge not found - PNG/ICO step skipped"); return
    svg = os.path.join(OUT, "favicon.svg")
    with tempfile.TemporaryDirectory() as td:
        for n in sizes:
            html = os.path.join(td, f"f{n}.html")
            with open(html, "w") as f:
                f.write(f'<html><body style="margin:0;background:transparent">'
                        f'<img src="file:///{svg.replace(os.sep, "/")}" style="width:{n}px;height:{n}px;display:block"></body></html>')
            png = os.path.join(OUT, f"favicon-{n}.png")
            subprocess.run([edge, "--headless", "--disable-gpu", "--default-background-color=00000000",
                            f"--window-size={n},{n}", f"--screenshot={png}", "file:///" + html.replace(os.sep, "/")],
                           stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=False)
            print("wrote", os.path.basename(png))
    try:
        from PIL import Image
    except ImportError:
        print("Pillow missing - favicon.ico not rebuilt"); return
    base = Image.open(os.path.join(OUT, "favicon-64.png")).convert("RGBA")
    others = [Image.open(os.path.join(OUT, f"favicon-{n}.png")).convert("RGBA") for n in (48, 32, 16)]
    ico = os.path.join(OUT, "favicon.ico")
    base.save(ico, format="ICO", sizes=[(64, 64), (48, 48), (32, 32), (16, 16)], append_images=others, bitmap_format="bmp")
    shutil.copy(ico, os.path.join(WWWROOT, "favicon.ico"))
    shutil.copy(os.path.join(OUT, "favicon-32.png"), os.path.join(WWWROOT, "favicon.png"))
    print("wrote favicon.ico (+ wwwroot/favicon.ico, favicon.png)")


if __name__ == "__main__":
    main()
