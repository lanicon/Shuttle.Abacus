import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import state from '~/state';
import Api from 'shuttle-can-api';
import localisation from '~/localisation';
import { MatrixMap } from '~/matrix/';
import { MatrixConstraintList } from '~/matrix-constraint/';
import { MatrixElementMap, MatrixElementList } from '~/matrix-element/';
import $ from 'jquery';

resources.add('matrix', {item: 'element', action: 'list', permission: Permissions.Manage.Matrices});

const ConstraintMap = DefineMap.extend({
    id: {
        type: 'string'
    },
    axis: {
        type: 'string'
    },
    comparison: {
        type: 'string'
    },
    value: {
        type: 'string'
    },
    viewModel: {
        Type: ViewModel
    },
    remove () {
        api.elements.delete({id: this.id})
            .then(function () {
                state.removalRequested('matrix-element');
            });
    },
    edit () {
        this.viewModel.element = this;
    }
});

const api = {
    matrices: new Api({
        endpoint: 'matrices/{id}',
        Map: MatrixMap
    }),
    constraints: new Api({
        endpoint: 'matrices/{id}/constraints'
    }),
    elements: new Api({
        endpoint: 'matrices/{id}/elements'
    })
};

export const ViewModel = DefineMap.extend({
    name: {
        type: 'string',
        default: ''
    },

    matrixId: {
        get () {
            return state.routeData.id;
        }
    },

    columns: {
        get () {
            var result = new DefineList();

            for (var i = 1; i <= this.requiredColumnCount(); i++) {
                result.push({index: i});
            }

            return result;
        }
    },

    rows: {
        get () {
            var result = new DefineList();

            for (var i = 1; i <= this.requiredRowCount(); i++) {
                result.push({index: i});
            }

            return result;
        }
    },

    requiredRowCount () {
        if (!this.constraints ||
            !this.elements) {
            return 0;
        }

        return this.constraints.reduce((current, item) => {
                return item.axis.toLowerCase() === 'row' ? item.index > current ? item.index : current : current;
            },
            this.elements.reduce((current, item) => {
                return item.row > current ? item.row : current;
            }, 0));
    },

    requiredColumnCount () {
        if (!this.constraints) {
            return 1;
        }

        return this.constraints.reduce((current, item) => {
                return item.axis.toLowerCase() === 'column' ? item.index > current ? item.index : current : current;
            }, 1);
    },

    hasValue(row, column) {
        var element = this.findElement(row.index, column.index);

        return !!element && !!element.value;
    },

    getValue(row, column) {
        var element = this.findElement(row.index, column.index);

        return !!element && !!element.value ? element.value : localisation.value('matrix-element-empty');
    },

    findConstraint (axis, index) {
        if (!this.constraints){
            return undefined;
        }

        const result = this.constraints.filter((item) =>{
           return item.axis.toLowerCase() === axis.toLowerCase() && item.index === index;
        });

        return !!result.length ? result[0] : undefined;
    },

    findElement (row, column) {
        if (!this.elements){
            return undefined;
        }

        const result = this.elements.filter((item) =>{
           return item.row === row && item.column === column;
        });

        return !!result.length ? result[0] : undefined;
    },

    getComparisonDisplay(axis, item) {
        const constraint = this.findConstraint(axis, item.index);

        return !!constraint ? constraint.getComparisonDisplay() : localisation.value('constraint-not-found');
    },

    elementValue: {
        type: 'string'
    },

    elementSelected: {
        type: 'boolean',
        get(){
            return !!this.element;
        }
    },

    editing(row, column) {
        return !!this.element ? this.element.is(row.index, column.index) : false;
    },

    edit(row, column) {
        this.element = this.findElement(row.index, column.index);

        if (!this.element){
            this.element = new MatrixElementMap({
                row: row.index,
                column: column.index
            });

            this.elements.push(this.element);
        }

        this.elementValue = this.element.value;

        $('#' + this.element.elementId).focus();
    },

    keyup(ev){
        if (ev.keyCode == 13) {
            this.save();
        }

        if (ev.keyCode == 27) {
            this.cancelEdit();
        }
    },

    cancelEdit(){
        this.element.value = this.elementValue;
        this.element = undefined;
    },

    save(){
        const self = this;

        if (!this.element || !this.element.value){
            this.element = undefined;
            return;
        }

        api.elements.post({
            row: this.element.row,
            column: this.element.column,
            value: this.element.value
        }, {
            id: this.matrix.id
        })
            .then(function () {
                state.registrationRequested('matrix-element');
                self.element = undefined;
            });
    },

    refreshTimestamp: {
        type: 'string'
    },

    matrix: {
        Type: MatrixMap
    },

    element: {
        Type: MatrixElementMap
    },

    constraints: {
        Type: MatrixConstraintList
    },

    elements: {
        Type: MatrixElementList
    },

    get map () {
        const self = this;
        const refreshTimestamp = this.refreshTimestamp;

        if (!this.matrixId) {
            return undefined;
        }

        return api.matrices.map({
            id: this.matrixId
        })
            .then(function (map) {
                self.matrix = map;
            })
            .finally(function () {
                api.elements.list({
                    id: self.matrix.id
                }).then(function (result) {
                    self.elements = new MatrixElementList(result);
                });

                api.constraints.list({
                    id: self.matrix.id
                }).then(function (result) {
                    self.constraints = new MatrixConstraintList(result);
                });
            });
    },

    init () {
        state.title = 'matrix-elements';

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    refresh: function () {
        this.refreshTimestamp = Date.now();
    }
});

export default Component.extend({
    tag: 'abacus-matrix-element-list',
    ViewModel,
    view
});