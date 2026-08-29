@extends('layouts/main')

@section('content')
<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="card-title">Add Exemption Inventory Approval / Min Sheet</h5>
                    </div>
                    <div class="card-body">

                        <form action="{{ route('exemption_inventory.store') }}" method="POST" enctype="multipart/form-data">
                            @csrf

                            <!-- Inventory Calculation Section -->
                            <div class="card mb-3">
                                <div class="card-header bg-light">
                                    <h6 class="mb-0">Inventory Calculation</h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <label class="form-label">Doc No</label>
                                                <input type="text" name="doc_no" class="form-control @error('doc_no') is-invalid @enderror" readonly value="{{$doc_num+1}}" required="">
                                                @error('doc_no')<span class="invalid-feedback">{{ $message }}</span>@enderror
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <?php
                                                $dt = new DateTime();
                                                ?>
                                                <label class="form-label">Date</label>
                                                <input type="date" name="date" class="form-control @error('date') is-invalid @enderror" readonly value="{{$dt->format('Y-m-d')}}" required>
                                                @error('date')<span class="invalid-feedback">{{ $message }}</span>@enderror
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">

                                        <div class="col-md-3">
                                            <label class="form-label" for="land_offer_form_no">Land Form NO</label>
                                            <select name="land_offer_form_no" id="land_offer_form_no" class="form-control" required>
                                                <option value="">Kindly Select</option>

                                                @foreach($land_owner as $row)
                                                <option value="{{ $row->doc_no }}">
                                                    Land Form No - {{ $row->doc_no }}
                                                </option>
                                                @endforeach
                                            </select>

                                            @error('land_offer_form_no')
                                            <div style="width: 100%; margin-top: 0.25rem; font-size: 75%; color: var(--lm-danger);">
                                                {{ $message }}
                                            </div>
                                            @enderror
                                        </div>

                                        <div class="col-md-3">
                                            <div class="mb-3">
                                                <label class="form-label">Total Registered Land</label>
                                                <input type="number" readonly step="0.0001" name="total_registered_land" class="form-control" value="{{ old('total_registered_land') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="mb-3">
                                                <label class="form-label">Total Possessed Land</label>
                                                <input type="number" readonly step="0.0001" name="total_possessed_land" class="form-control" value="{{ old('total_possessed_land') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="mb-3">
                                                <label class="form-label">Rate / Acre (Mn)</label>
                                                <input type="number" readonly step="0.01" name="rate_per_acre" class="form-control" value="{{ old('rate_per_acre') }}">
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <label class="form-label">Total Cost (as per Registered Land) Mn</label>
                                                <input type="number" readonly step="0.01" name="total_cost_registered" class="form-control" value="{{ old('total_cost_registered') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="mb-3">
                                                <label class="form-label">Total Cost (as per Possessed Land) Mn</label>
                                                <input type="number" readonly step="0.01" name="total_cost_possessed" class="form-control" value="{{ old('total_cost_possessed') }}">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Inventory Details Section -->
                            <div class="card mb-3">
                                <div class="card-header bg-light">
                                    <h6 class="mb-0">Inventory Details</h6>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <table class=" table-sm table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Category</th>
                                                    <th>Type</th>
                                                    <th>Size of File/Plot</th>
                                                    <th>No. of Files/Plots</th>
                                                    <th>Rate per File/Plot</th>
                                                    <th>Total Cost</th>
                                                    <th>80%</th>
                                                    <th>20%</th>
                                                    <th>Remark</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody id="inventoryBody">
                                                <tr class="inventory-row">
                                                    <td>
                                                        <select name="inventory_lines[0][category]" class="form-select form-select-sm">
                                                            <option value="">Select</option>
                                                            <option value="Residential">Residential</option>
                                                            <option value="Commercial">Commercial</option>
                                                            <option value="Cash">Cash</option>
                                                            <option value="Decimal">Decimal</option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <select name="inventory_lines[0][inventory_type]" class="form-select form-select-sm">
                                                            <option value="">Select</option>
                                                            <option value="Files">Files</option>
                                                            <option value="Plots">Plots</option>
                                                        </select>
                                                    </td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][size_of_file]" class="form-control form-control-sm"></td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][no_of_files]" class="form-control form-control-sm"></td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][rate_file_plot]" class="form-control form-control-sm"></td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][total_cost]" class="form-control form-control-sm total-cost-field" readonly></td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][eighty_percent]" class="form-control form-control-sm"></td>
                                                    <td><input type="number" step="0.01" name="inventory_lines[0][twenty_percent]" class="form-control form-control-sm"></td>
                                                    <td><input type="text" name="inventory_lines[0][remark]" class="form-control form-control-sm"></td>
                                                    <td>
                                                        <button type="button" class="btn btn-sm btn-danger" onclick="removeRow(this)">
                                                            <i class="fas fa-trash"></i>
                                                        </button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <tr style="background: var(--lm-surface);font-weight:bold">
                                                    <td colspan="2" style="text-align:right;">TOTAL</td>

                                                    <!--  Size Total -->
                                                    <td>
                                                        <input type="number" id="total_size" class="form-control form-control-sm" readonly>
                                                    </td>

                                                    <!-- Qty -->
                                                    <td>
                                                        <input type="number" id="total_qty" class="form-control form-control-sm" readonly>
                                                    </td>

                                                    <td></td>

                                                    <!-- Total Cost -->
                                                    <td>
                                                        <input type="number" id="total_cost_sum" class="form-control form-control-sm" readonly>
                                                    </td>

                                                    <!-- 80% -->
                                                    <td>
                                                        <input type="number" id="total_80" class="form-control form-control-sm" readonly>
                                                    </td>

                                                    <!-- 20% -->
                                                    <td>
                                                        <input type="number" id="total_20" class="form-control form-control-sm" readonly>
                                                    </td>

                                                    <td colspan="2"></td>
                                                </tr>
                                            </tfoot>

                                        </table>
                                    </div>
                                    <button type="button" class="btn btn-sm btn-success" onclick="addInventoryRow()">
                                        <i class="fas fa-plus"></i> Add Row
                                    </button>
                                </div>
                            </div>

                            <!-- Summary Section -->
                            <div class="card mb-3">
                                <div class="card-header bg-light">
                                    <h6 class="mb-0">Summary</h6>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Total Residential Files</label>
                                                <input type="number" readonly step="0.01" name="total_residential_files" class="form-control" value="{{ old('total_residential_files') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Total Commercial Files</label>
                                                <input type="number" readonly step="0.01" name="total_commercial_files" class="form-control" value="{{ old('total_commercial_files') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Total Marlas</label>
                                                <input type="number" readonly step="0.01" name="total_marlas" class="form-control">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Exemption %</label>
                                                <input type="number" step="0.01" name="exemption_percent" class="form-control" value="{{ old('exemption_percent') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Total Cost (Mn)</label>
                                                <input type="number" readonly step="0.01" name="total_cost" class="form-control" value="{{ old('total_cost') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Residential %</label>
                                                <input type="number" readonly step="0.01" name="residential_percent" class="form-control" value="{{ old('residential_percent') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Commercial %</label>
                                                <input type="number" readonly step="0.01" name="commercial_percent" class="form-control" value="{{ old('commercial_percent') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Cash</label>
                                                <input type="number" step="0.01" name="cash" class="form-control" value="{{ old('cash') }}">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="mb-3">
                                                <label class="form-label">Decimal</label>
                                                <input type="number" step="0.01" name="inv_decimal" class="form-control" value="{{ old('inv_decimal') }}">
                                            </div>
                                        </div>
                                    </div>



                                </div>
                            </div>

                            <!-- Remarks Section -->
                            <div class="card mb-3">
                                <div class="card-header bg-light">
                                    <h6 class="mb-0">Remarks</h6>
                                </div>
                                <div class="card-body">
                                    <div class="mb-3">
                                        <textarea name="remarks" class="form-control" rows="4" placeholder="Enter remarks...">{{ old('remarks') }}</textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">

                                <div class="mb-3">
                                    <label class="form-label"
                                        for="attachment">Attachment</label>
                                    <input class="form-control"
                                        id="attachment"
                                        name="attachment"
                                        type="file"
                                        value="{{ old('attachment') }}"
                                        required="" />
                                </div>

                            </div>

                            <!-- Form Actions -->
                            <div class="mb-3">
                                <button type="submit" class="btn btn-primary">
                                    <i class="fas fa-save"></i> Save
                                </button>
                                <a href="{{ route('exemption_inventory.index') }}" class="btn btn-secondary">
                                    <i class="fas fa-arrow-left"></i> Back
                                </a>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script>
    let rowCount = 1;

    function calculateFooterTotals() {
        let totalSize = 0;
        let totalQty = 0;
        let totalCost = 0;
        let total80 = 0;
        let total20 = 0;

        document.querySelectorAll('#inventoryBody tr').forEach(row => {

            const size = parseFloat(row.querySelector('[name*="[size_of_file]"]').value) || 0;
            const qty = parseFloat(row.querySelector('[name*="[no_of_files]"]').value) || 0;
            const cost = parseFloat(row.querySelector('[name*="[total_cost]"]').value) || 0;
            const eighty = parseFloat(row.querySelector('[name*="[eighty_percent]"]').value) || 0;
            const twenty = parseFloat(row.querySelector('[name*="[twenty_percent]"]').value) || 0;

            totalSize += size; //  NEW
            totalQty += qty;
            totalCost += cost;
            total80 += eighty;
            total20 += twenty;
        });

        document.getElementById('total_size').value = totalSize.toFixed(2); //  NEW
        document.getElementById('total_qty').value = totalQty.toFixed(2);
        document.getElementById('total_cost_sum').value = totalCost.toFixed(2);
        document.getElementById('total_80').value = total80.toFixed(2);
        document.getElementById('total_20').value = total20.toFixed(2);
    }

    /* =========================
       🔹 MAIN CALCULATIONS
    ========================= */

    // Land Cost Calculation
    function calculateTotalCosts() {
        const totalRegisteredLand = parseFloat(document.querySelector('[name="total_registered_land"]').value) || 0;
        const totalPossessedLand = parseFloat(document.querySelector('[name="total_possessed_land"]').value) || 0;
        const ratePerAcre = parseFloat(document.querySelector('[name="rate_per_acre"]').value) || 0;

        document.querySelector('[name="total_cost_registered"]').value = (totalRegisteredLand * ratePerAcre).toFixed(2);
        document.querySelector('[name="total_cost_possessed"]').value = (totalPossessedLand * ratePerAcre).toFixed(2);

        calculateSummary();
    }

    /* =========================
       🔹 ROW CALCULATION
    ========================= */

    function calculateRow(row) {
        const qty = parseFloat(row.querySelector('[name*="[no_of_files]"]').value) || 0;
        const rate = parseFloat(row.querySelector('[name*="[rate_file_plot]"]').value) || 0;

        const total = qty * rate;
        const eighty = total * 0.8;
        const twenty = total * 0.2;

        row.querySelector('[name*="[total_cost]"]').value = total.toFixed(2);
        row.querySelector('[name*="[eighty_percent]"]').value = eighty.toFixed(2);
        row.querySelector('[name*="[twenty_percent]"]').value = twenty.toFixed(2);

        calculateSummary();
        calculateMarlas();
        calculateFooterTotals();
    }

    function calculateMarlas() {
        let totalMarlas = 0;

        document.querySelectorAll('#inventoryBody tr').forEach(row => {
            const size = parseFloat(row.querySelector('[name*="[size_of_file]"]').value) || 0;
            const qty = parseFloat(row.querySelector('[name*="[no_of_files]"]').value) || 0;

            totalMarlas += (size * qty);
        });

        document.querySelector('[name="total_marlas"]').value = totalMarlas.toFixed(2);
    }


    /* =========================
       🔹 SUMMARY CALCULATION
    ========================= */

    function calculateSummary() {
        let totalCost = 0;
        let residentialCost = 0;
        let commercialCost = 0;
        let cashTotal = 0;
        let decimalTotal = 0;

        let residentialFiles = 0;
        let commercialFiles = 0;

        let residentialMarlas = 0;
        let commercialMarlas = 0;
        let totalMarlas = 0;

        document.querySelectorAll('#inventoryBody tr').forEach(row => {
            const category = row.querySelector('[name*="[category]"]').value;
            const total = parseFloat(row.querySelector('[name*="[total_cost]"]').value) || 0;
            const qty = parseFloat(row.querySelector('[name*="[no_of_files]"]').value) || 0;
            const size = parseFloat(row.querySelector('[name*="[size_of_file]"]').value) || 0;

            const marlas = size * qty;

            totalCost += total;
            totalMarlas += marlas;

            if (category === 'Residential') {
                residentialCost += total;
                residentialFiles += qty;
                residentialMarlas += marlas;
            }

            if (category === 'Commercial') {
                commercialCost += total;
                commercialFiles += qty;
                commercialMarlas += marlas;
            }

            if (category === 'Cash') {
                cashTotal += total;
            }

            if (category === 'Decimal') {
                decimalTotal += qty;
            }
        });

        // Fill basic fields
        document.querySelector('[name="total_cost"]').value = totalCost.toFixed(2);
        document.querySelector('[name="total_residential_files"]').value = residentialFiles;
        document.querySelector('[name="total_commercial_files"]').value = commercialFiles;
        document.querySelector('[name="total_marlas"]').value = totalMarlas.toFixed(2);
        document.querySelector('[name="cash"]').value = cashTotal.toFixed(2);
        document.querySelector('[name="inv_decimal"]').value = decimalTotal.toFixed(2);

        // 🔥 NEW FORMULA
        const totalRegisteredLand = parseFloat(document.querySelector('[name="total_registered_land"]').value) || 0;

        let exemption = 0;
        let resPercent = 0;
        let comPercent = 0;

        if (totalRegisteredLand > 0) {

            // Exemption %
            exemption = ((totalMarlas / 20) / totalRegisteredLand / 8) * 100;

            // Residential %
            resPercent = ((residentialMarlas / 20) / totalRegisteredLand / 8) * 100;

            // Commercial %
            comPercent = ((commercialMarlas / 20) / totalRegisteredLand / 8) * 100;
        }

        document.querySelector('[name="exemption_percent"]').value = exemption.toFixed(2);
        document.querySelector('[name="residential_percent"]').value = resPercent.toFixed(2);
        document.querySelector('[name="commercial_percent"]').value = comPercent.toFixed(2);

        if (typeof validateLive === 'function') {
            validateLive(); // for edit page safety
        }
    }

    /* =========================
       🔹 VALIDATION
    ========================= */

    function validateForm() {
        const totalCost = parseFloat(document.querySelector('[name="total_cost"]').value) || 0;
        const possessedCost = parseFloat(document.querySelector('[name="total_cost_possessed"]').value) || 0;

        if (totalCost !== possessedCost) {
            alert(' Inventory Total Cost must match Total Possessed Land Cost');
            return false;
        }

        return true;
    }

    /* =========================
       🔹 HANDLE CATEGORY CHANGE
    ========================= */

    function handleCategoryChange(categorySelect) {
        const row = categorySelect.closest('tr');
        const totalCostField = row.querySelector('.total-cost-field');
        const category = categorySelect.value;

        // Make editable for Cash and Decimal, readonly for Residential and Commercial
        if (category === 'Cash' || category === 'Decimal') {
            totalCostField.removeAttribute('readonly');
            totalCostField.style.backgroundColor = '#fff';
        } else {
            totalCostField.setAttribute('readonly', 'readonly');
            totalCostField.style.backgroundColor = '#e9ecef';
            // Recalculate if residential or commercial
            calculateRow(row);
        }
    }

    /* =========================
       🔹 EVENTS (AUTO TRIGGER)
    ========================= */

    // Auto calculation on input
    document.addEventListener('input', function(e) {

        // Land inputs
        if (
            e.target.name === 'total_registered_land' ||
            e.target.name === 'total_possessed_land' ||
            e.target.name === 'rate_per_acre'
        ) {
            calculateTotalCosts();
        }

        // Inventory row inputs
        if (
            e.target.name.includes('[no_of_files]') ||
            e.target.name.includes('[rate_file_plot]') ||
            e.target.name.includes('[size_of_file]')
        ) {
            calculateRow(e.target.closest('tr'));
            calculateFooterTotals();
        }

        // Handle Total Cost field input for Cash/Decimal (manual entry)
        if (e.target.name.includes('[total_cost]') && e.target.classList.contains('total-cost-field')) {
            const row = e.target.closest('tr');
            const category = row.querySelector('[name*="[category]"]').value;
            
            if (category === 'Cash' || category === 'Decimal') {
                // User manually entered a value, just update summaries
                calculateSummary();
                calculateFooterTotals();
            }
        }

    });

    /* =========================
       🔹 FETCH LAND FORM
    ========================= */

    document.getElementById('land_offer_form_no').addEventListener('change', function() {
        const docNo = this.value;

        if (docNo) {
            fetch(`/test_Land_mgt/get-land-form-details/${docNo}`)
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        document.querySelector('[name="rate_per_acre"]').value = data.rate_per_acre || '';
                        document.querySelector('[name="total_registered_land"]').value = data.total_registered_land || '';
                        document.querySelector('[name="total_possessed_land"]').value = data.total_possessed_land || '';
                        calculateTotalCosts();
                    }
                });
        }
    });

    /* =========================
       🔹 CATEGORY CHANGE EVENT
    ========================= */

    document.addEventListener('change', function(e) {
        if (e.target.name.includes('[category]')) {
            handleCategoryChange(e.target);
        }
    });

    /* =========================
       🔹 ADD / REMOVE ROW
    ========================= */

    function addInventoryRow() {
        const tbody = document.getElementById('inventoryBody');

        const newRow = document.createElement('tr');
        newRow.innerHTML = `
        <td>
            <select name="inventory_lines[${rowCount}][category]" class="form-select form-select-sm category-select">
                <option value="">Select</option>
                <option value="Residential">Residential</option>
                <option value="Commercial">Commercial</option>
                <option value="Cash">Cash</option>
                <option value="Decimal">Decimal</option>
            </select>
        </td>
        <td>
            <select name="inventory_lines[${rowCount}][inventory_type]" class="form-select form-select-sm">
                <option value="">Select</option>
                <option value="Files">Files</option>
                <option value="Plots">Plots</option>
            </select>
        </td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][size_of_file]" class="form-control form-control-sm"></td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][no_of_files]" class="form-control form-control-sm"></td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][rate_file_plot]" class="form-control form-control-sm"></td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][total_cost]" class="form-control form-control-sm total-cost-field" readonly></td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][eighty_percent]" class="form-control form-control-sm"></td>
        <td><input type="number" step="0.01" name="inventory_lines[${rowCount}][twenty_percent]" class="form-control form-control-sm"></td>
        <td><input type="text" name="inventory_lines[${rowCount}][remark]" class="form-control form-control-sm"></td>
        <td>
            <button type="button" class="btn btn-sm btn-danger" onclick="removeRow(this)">
                <i class="fas fa-trash"></i>
            </button>
        </td>
    `;

        tbody.appendChild(newRow);
        
        // Add event listener for category change in the new row
        newRow.querySelector('.category-select').addEventListener('change', function() {
            handleCategoryChange(this);
        });
        
        rowCount++;
        calculateFooterTotals();
    }

    function removeRow(btn) {
        btn.closest('tr').remove();
        calculateSummary();
        calculateMarlas();
        calculateFooterTotals();
    }

    /* =========================
       🔹 FORM SUBMIT VALIDATION
    ========================= */

    document.querySelector('form').addEventListener('submit', function(e) {
        if (!validateForm()) {
            e.preventDefault();
        }
    });
    window.addEventListener('load', function() {
        calculateFooterTotals();
    });
</script>
@endsection