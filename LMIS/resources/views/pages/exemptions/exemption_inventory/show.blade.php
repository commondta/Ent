@extends('layouts/main')

@section('content')
<div class="content">
    <div class="mt-4">
        <div class="row g-4">
            <div class="col-12 col-xl-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="card-title">Exemption Inventory Approvals</h5>
                        @if(auth()->user()->exemption_inventory_add == 1 || auth()->user()->is_admin == 1)
                        <a href="{{ route('exemption_inventory.create') }}" class="btn btn-primary btn-sm">
                            <i class="fas fa-plus"></i> Add New
                        </a>
                        @endif
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-striped table-sm">
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>Doc No</th>
                                        <th>Date</th>
                                        <th>Land Offer Form No</th>
                                        <th>Total Cost</th>
                                        <!-- <th>Status</th> -->
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    @forelse($record as $row)
                                    <tr>
                                        <td>{{ $loop->iteration }}</td>
                                        <td>{{ $row->doc_no }}</td>
                                        <td>{{ $row->date }}</td>
                                        <td>{{ $row->land_offer_form_no }}</td>
                                        <td>{{ $row->total_cost }}</td>
                                        <!-- <td>
                                            @if($row->status == 0)
                                            <span class="badge bg-success">Approved</span>
                                            @elseif($row->status == 1)
                                            <span class="badge bg-warning">Pending</span>
                                            @endif
                                        </td> -->
                                        <td>
                                            <div class="btn-group btn-group-sm" role="group">
                                                @if(auth()->user()->exemption_inventory_edit == 1 || auth()->user()->is_admin == 1)
                                                <a href="{{ route('exemption_inventory.edit', $row->id) }}" class="btn btn-info">
                                                    <i class="fas fa-edit"></i>
                                                </a>
                                                @endif
                                                @if(auth()->user()->exemption_inventory_delete == 1 || auth()->user()->is_admin == 1)
                                                <form action="{{ route('exemption_inventory.destroy', $row->id) }}" method="POST" style="display:inline;">
                                                    @csrf
                                                    @method('DELETE')
                                                    <button type="submit" class="btn btn-danger" onclick="return confirm('Are you sure?')">
                                                        <i class="fas fa-trash"></i>
                                                    </button>
                                                </form>
                                                @endif
                                            </div>
                                        </td>
                                    </tr>
                                    @empty
                                    <td></td>
                                    <td></td>
                                   
                                    <td class="text-center">No records found</td>
                                    <td></td>
                                    <td></td>
                                    <td></td>

                                    @endforelse
                                </tbody>
                            </table>
                        </div>
                        <div class="d-flex justify-content-center">
                            {{ $record->links() }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection