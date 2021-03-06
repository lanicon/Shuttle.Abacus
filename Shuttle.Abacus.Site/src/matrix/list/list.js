import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import localisation from '~/localisation';
import state from '~/state';
import Api from 'shuttle-can-api';

resources.add('matrix', { action: 'list', permission: Permissions.Manage.Matrices});

export const Map = DefineMap.extend({
	constraints() {
		router.goto({
			resource: 'matrix',
			item: 'constraint',
			action: 'list',
			id: this.id
		});
	},
	elements() {
		router.goto({
			resource: 'matrix',
			item: 'element',
			action: 'list',
			id: this.id
		});
	}
});

export const api = {
    matrices: new Api({
        endpoint: 'matrices/{id}'
    }),
    search: new Api({
        endpoint: 'matrices/search',
        Map
    })
};

export const ViewModel = DefineMap.extend({
    name: {
        type: 'string',
        default: ''
    },

    columns: {
        Default: DefineList
    },

    refreshTimestamp: {
        type: 'string'
    },

    get list () {
        const refreshTimestamp = this.refreshTimestamp;
        return api.search.list({
            name: this.name
        }, {
            post: true
        });
    },

    init() {
        const columns = this.columns;

        if (!columns.length) {
	        columns.push({
		        columnTitle: 'constraints',
		        columnClass: 'col-1',
		        stache: '<cs-button text:from="\'constraints\'" click:from="constraints" elementClass:from="\'btn-sm\'"/>'
	        });

	        columns.push({
                columnTitle: 'elements',
                columnClass: 'col-1',
                stache: '<cs-button text:from="\'elements\'" click:from="elements" elementClass:from="\'btn-sm\'"/>'
            });

            columns.push({
                columnTitle: 'name',
                columnClass: 'col',
                attributeName: 'name'
            });

            columns.push({
                columnTitle: 'data-type-name',
                columnClass: 'col',
                attributeName: 'dataTypeName'
            });

            columns.push({
                columnTitle: 'row-argument',
                columnClass: 'col',
                attributeName: 'rowArgumentName'
            });

            columns.push({
                columnTitle: 'column-argument',
                columnClass: 'col',
                attributeName: 'columnArgumentName'
            });
        }

        state.title = 'matrices';

        state.navbar.addButton({
            type: 'add',
            viewModel: this,
            permission: Permissions.Manage.Matrices
        });

        state.navbar.addButton({
            type: 'refresh',
            viewModel: this
        });
    },

    add: function() {
        router.goto({
            resource: 'matrix',
            action: 'add'
        });
    },

    refresh: function() {
        this.refreshTimestamp = Date.now();
    }
});

export default Component.extend({
    tag: 'abacus-matrix-list',
    ViewModel,
    view
});