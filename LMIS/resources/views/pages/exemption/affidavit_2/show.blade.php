@extends('layouts/main')

@section('content')

    <div class="content">
        <div class="mt-4">
            <div class="row g-4">
                <div class="col-12 col-xl-12 order-1 order-xl-0">
                    <div class="mb-9">
                        @if(session('success'))

                            <div class="alert alert-outline-success d-flex align-items-center" role="alert">
                                <span class="fas fa-check-circle text-success fs-3 me-3"></span>

                                <p class="mb-0 flex-1">{{ session('success') }}</p>
                                <button class="btn-close" type="button" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>

                        @endif
                        @if(session('danger'))
                            <div class="alert alert-outline-danger d-flex align-items-center" role="alert">
                                <span class="fas fa-times-circle text-danger fs-3 me-3"></span>

                                <p class="mb-0 flex-1">{{ session('danger') }}</p>
                                <button class="btn-close" type="button" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>
                        @endif
                        <div class="card shadow-none border border-300 my-4" data-component-card="data-component-card">
                            <div class="card-header p-4 border-bottom border-300 bg-soft">
                                <div class="row g-3 justify-content-between align-items-end">
                                    <div class="col-12 col-md">
                                        <h4 class="text-900 mb-0" data-anchor="data-anchor">Affidavit</h4>
                                    </div>
                                    @if(auth()->user()->affidavit_2_add == 1)

                                        <div class="col col-md-auto">
                                            <nav class="nav nav-underline justify-content-end doc-tab-nav align-items-center"
                                                role="tablist">

                                                <a href="{{ route('affidavit_2.create') }}"
                                                    class="btn btn-sm btn-phoenix-primary preview-btn ms-2"
                                                    style="width: 190px">
                                                    <span class="me-2" data-feather="plus"></span>Add New Record</a>
                                            </nav>
                                        </div>
                                    @endif
                                </div>
                            </div>
                            <div class="card-body p-0">

                                <div class="p-4 code-to-copy">

                                    <div class="table-responsive">
                                        <table class="table table-striped table-sm fs--1 mb-0">
                                            <thead>
                                                <tr>
                                                    <th class="border-top ps-3">Doc NO</th>
                                                    <th class="border-top ps-3">Base Doc NO</th>
                                                    <th class="border-top ps-3">Doc Date</th>

                                                    <th>
                                                        ACTION
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                @foreach($record as $row)
                                                    <tr>
                                                        <td class="align-middle ps-3">{{ $row->doc_no }}</td>
                                                        <td class="align-middle ps-3">{{ $row->base_doc_no }}</td>
                                                        <td class="align-middle ps-3">{{ $row->date }}</td>

                                                        <td class="align-middle white-space-nowrap text-end pe-0">
                                                            <div class="font-sans-serif btn-reveal-trigger position-static">
                                                                <button
                                                                    class="btn btn-sm dropdown-toggle dropdown-caret-none transition-none btn-reveal fs--2"
                                                                    type="button" data-bs-toggle="dropdown"
                                                                    data-boundary="window" aria-haspopup="true"
                                                                    aria-expanded="false" data-bs-reference="parent"><span
                                                                        class="fas fa-ellipsis-h fs--2"></span>
                                                                </button>
                                                                <div class="dropdown-menu dropdown-menu-end py-2">
                                                                    @if(auth()->user()->affidavit_2_edit == 1)

                                                                        <a class="dropdown-item"
                                                                            href="{{ route('affidavit_2.edit', $row->id) }}">Edit</a>

                                                                    @endif
                                                                    @if(auth()->user()->affidavit_2_print == 1)

                                                                        <a target="_blank" class="dropdown-item"
                                                                            href="{{ route('affidavit_2.show', $row->id) }}">Layout</a>
                                                                    @endif
                                                                    @if(auth()->user()->affidavit_2_list == 1)

                                                                        <button type="button" class="dropdown-item"
                                                                            onclick="ViewHistory('<?php        echo $row->id; ?>','Affidavit 2')">View
                                                                            History</button>
                                                                    @endif
                                                                    @if(auth()->user()->affidavit_2_edit == 1)

                                                                        <form id="deleteForm-{{ $row->id }}"
                                                                            action="{{ route('affidavit_2.destroy', $row->id) }}"
                                                                            method="Post">
                                                                            @csrf
                                                                            @method('DELETE')
                                                                            <div class="dropdown-divider"></div>
                                                                            <button onclick="confirmSubmit(); return false;"
                                                                                class="dropdown-item text-danger">
                                                                                Remove
                                                                            </button>
                                                                            {{--<a type="submit" class="dropdown-item text-danger"
                                                                                href="#!"></a>--}}

                                                                        </form>
                                                                    @endif
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                @endforeach

                                            </tbody>
                                        </table>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="position-fixed bottom-0 end-0 p-3" style="z-index: 5">
            <div class="toast align-items-center text-white bg-dark border-0 light" id="icon-copied-toast" role="alert"
                aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body p-3"></div>
                    <button class="btn-close btn-close-white me-2 m-auto" type="button" data-bs-dismiss="toast"
                        aria-label="Close"></button>
                </div>
            </div>
        </div>
        <footer class="footer position-absolute">
            <div class="row g-0 justify-content-between align-items-center h-100">
                <div class="col-12 col-sm-auto text-center">
                    <p class="mb-0 mt-2 mt-sm-0 lm-footer-text"><span class="lm-footer-brand">Land Information Management System</span><span class="lm-footer-sep">|</span><span>&copy; {{ date('Y') }}</span><span class="lm-footer-sep">|</span><span>Powered by <img src="{{ asset('public/assets/img/n-stack-logo.png') }}" alt="" class="lm-footer-logo"> <strong>N-Stack</strong></span></p>
                </div>
                <div class="col-12 col-sm-auto text-center">
                </div>
            </div>
        </footer>
    </div>

    <script>
        function confirmSubmit(id) {
            if (confirm('Are you sure you want to delete this record?')) {
                document.getElementById('deleteForm-' + id).submit();
            }
        }
    </script>
    <!-- Your content here -->
@endsection