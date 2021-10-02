
<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <p>Zoom: {{zoom}}, Resolution: {{resolution}}</p>
      <div id="map-wrap" style="height: 100vh">
      <client-only>
        <l-map 
          :zoom="zoom"
          :center="center"
          @ready="mapReady"
          @update:zoom="zoomUpdated"
          @update:center="centerUpdated"
          @update:bounds="boundsUpdated">
          <l-tile-layer :url="url" />
          <l-layer-group>
            <l-geo-json
              :options="options"
              :geojson="geojson"/>
          </l-layer-group>
        </l-map>
      </client-only>
      </div>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { LatLng, LatLngBounds } from "leaflet";
import { LMap, LTileLayer, LMarker, LGeoJson, LPopup, LTooltip } from "vue2-leaflet";

export default {
  components: {
    LMap,
    LTileLayer,
    LGeoJson,
    LMarker,
    LPopup,
    LTooltip
  },
  data() {
    return{
      center: [-27.492572512461365,153.00566130144634],
      url: 'http://{s}.tile.osm.org/{z}/{x}/{y}.png',
      fillColor: "#e4ce7f",
      zoom: 13,
      resolution: 10,
      geojson: [],
      bounds: [],
    }
   },
   methods: {
    zoomUpdated (zoom: number) {
      this.zoom = zoom;
      this.getGeoJson();
    },
    centerUpdated (center: LatLng) {
      this.center = center;
    },
    boundsUpdated (bounds: LatLngBounds) {
      this.bounds = bounds;
    },
    mapReady (map: any) {
      this.bounds = map.getBounds();
      this.getGeoJson();
    },
    async getGeoJson() {
      if (this.zoom === 10)
        this.resolution = 5;
      if (this.zoom === 11)
        this.resolution = 6;
      if (this.zoom === 12)
        this.resolution = 7;
      if (this.zoom === 13)
        this.resolution = 8;
      if (this.zoom === 14)
        this.resolution = 9;
      if (this.zoom === 15)
        this.resolution = 10;
      if (this.zoom === 16)
        this.resolution = 11;

      const bbox = [this.bounds.getEast(), this.bounds.getNorth(), this.bounds.getWest(), this.bounds.getSouth()];

      const geojson = await this.$axios.$get(`http://localhost:5000/data/cluster?zoom=${this.zoom}&resolution=${this.resolution}&bbox=${bbox}`);
      
      console.log('geojson', geojson);

      this.geojson = geojson;
    }
   },
   computed: {
    options() {
      return {
        onEachFeature: this.onEachFeatureFunction
      };
    },
    onEachFeatureFunction() {
      return (feature: any, layer: any) => {
        layer.bindTooltip(
          "<div>code:" + JSON.stringify(feature.properties) + "</div>",
          { permanent: false, sticky: true }
        );
      };
    }
  }
}
</script>