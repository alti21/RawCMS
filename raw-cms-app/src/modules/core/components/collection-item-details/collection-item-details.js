import { vuexStore } from '../../../../config/vuex.js';
import { optionalChain } from '../../../../utils/object.utils.js';
import { RawCmsDetailEditDef } from '../../../shared/components/detail-edit/detail-edit.js';
import { entitiesSchemaService } from '../../services/entities-schema.service.js';
import { validationService } from '../../services/validation.service.js';

const _CollectionItemDetailsWrapperDef = async () => {
  const rawCmsDetailEditDef = await RawCmsDetailEditDef();

  return {
    extends: rawCmsDetailEditDef,
  };
};

const _CollectionItemDetailsDef = async () => {
  const detailWrapperDef = await _CollectionItemDetailsWrapperDef();
  const tpl = await RawCMS.loadComponentTpl(
    '/modules/core/components/collection-item-details/collection-item-details.tpl.html'
  );

  return {
    components: {
      DetailWrapper: detailWrapperDef,
    },
    computed: {
      apiBasePath: function() {
        return `/api/CRUD/${this.collectionName}`;
      },
      fieldsMetadata: function() {
        return optionalChain(() => vuexStore.state.core.fieldsMetadata, {
          fallbackValue: [],
          replaceLastUndefined: true,
        });
      },
    },
    created: async function() {
      this.formFields = await this.getFormFields();
    },
    data: function() {
      return {
        entitiesSchemaService: entitiesSchemaService,
        formState: {},
        formFields: [],
      };
    },
    methods: {
      getFormFields: async function() {
        const res = await this.entitiesSchemaService.getPage({
          size: 1,
          rawQuery: { CollectionName: this.collectionName },
        });
        const schema = res.items[0];

        const result = schema.FieldSettings.map(x => {
          return this.applyFieldMetadata(
            {
              key: x.Name,
              type: x.Type,
              validators: {},
              templateOptions: { validation: {} },
              wrapper: '<div class="col-12 col-sm-6"></div>',
            },
            x
          );
        });

        return result;
      },
      applyFieldMetadata: function(formlyField, schemaField) {
        return validationService.applyFieldMetadataToFormlyInput(formlyField, {
          fieldType: schemaField.Type,
          options: schemaField.Options,
        });
      },
    },
    props: {
      collectionName: String,
    },
    template: tpl,
  };
};

const _CollectionItemDetails = async (res, rej) => {
  const cmpDef = _CollectionItemDetailsDef();
  res(cmpDef);
};

export const CollectionItemDetailsDef = _CollectionItemDetailsDef;
export const CollectionItemDetails = _CollectionItemDetails;
export default _CollectionItemDetails;
