## ESearch
<img src="img/logo.png" width="400px">  

ESearch is a simple search module for Sitecore XP. This project is created as a submission for [konabos Search UI Competition](https://www.konabos.com/search-ui-competition).  

**Warning: This software is still in the beta stage.**

## Prerequisites
- Sitecore Experience Platform 9.2

## Installation
1. Download the package `ESearch-{version}.zip` from [here](https://github.com/Sitecore-ESearch/ESearch/releases).
1. Install the package into your Sitecore via Installation Wizard.

### Sample Website
ESearch provides a sample website built with this module & Bootstrap 4.

![](img/Screenshot.png)  

To install the sample website:  

1. Install the ESearch module.
1. Install `ESearch - Sample Website-{version}.zip` (available in the [release page](https://github.com/Sitecore-ESearch/ESearch/releases)).
1. Execute site publishing.
1. Rebuild `sitecore_master_index` and `sitecore_web_index`.
1. Add a bind `esearch.example.com:80` to your site on IIS.
1. Add a DNS record `127.0.0.1 esearch.example.com` to the `hosts` file.
1. Access to `http://esearch.example.com` and enjoy it!

## Usage
This module contains the following components.

- <a href="#search-results">Search Results</a>
- <a href="#search-result-summary">Search Result Summary</a>
- <a href="#page-selector">Page Selector</a>
- <a href="#facet-filter">Facet Filter</a>
- <a href="#sort-indicator">Sort Indicator</a>
- <a href="#search-box">Search Box</a> (supports typeahead suggestion)

---

**NOTE**

You can see how to use these components on YouTube.

- [How to use the ESearch components](https://youtu.be/3pf_fGUj-5I)

---

These components have the CSS classes based on Bootstrap 4 in its HTML, but it's not required. You can use your own styles if you want.  

All components need to set a `Search Settings` item in the rendering parameter. The `Default Search Settings` is used by default.

- `Search Settings`:
  - Path: /sitecore/templates/Foundation/ESearch/Indexing/Search Settings
- `Default Search Settings`:
  - Path: /sitecore/system/Settings/ESearch/Search Settings/Default Search Settings

The `Search Settings` has following search options.

|Field name|Description|
|:-|:-|
|`Scope`|Root item for filtering the search results.|
|`Target templates`|Templates for filtering the search results.|
|`Page Size`|Number of items displayed in the search results component per page.|
|`Keyword Search Targets`|Fields used in the keyword search. This affects search performance, so less is better.|
|`Date Format`|Date format used for displaying date.|

### Search Results
The Search Results component shows the results of searching.  

![](img/Component_SearchResults.png)

A result item uses the `Search Result Data` template for displaying its information. Set this to your `Base template` field.  

- /sitecore/templates/Feature/ESearch/Search Result Data

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Search Results/Search Results

|Field name|Description|
|:-|:-|
|`Read More Label`|Text displayed on the "Read more" link in each result item.|

#### Rendering Parameters
- Path: /sitecore/templates/Feature/ESearch/Search Results/Rendering Parameter/Search Results Parameters

|Parameter name|Description|
|:-|:-|
|`Item Css Class`|Class names that set to each result item.|

### Search Result Summary
The Search Result Summary component shows the count of results and current search conditions.  
Search conditions are not displayed by default.

![](img/Component_SearchResultSummary.png)

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Search Result Summary/Search Result Summary 

|Field name|Description|
|:-|:-|
|`Total Count Label`|Text displayed on the title of count of result.|
|`Search Conditions Label`|Text displayed on the title of current search conditions.|

#### Rendering Parameters
No rendering parameters in this component.

### Page Selector
The Page Selector component shows the link list for the pagination.

![](img/Component_PageSelector.png)

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Page Selector/Page Selector

|Field name|Description|
|:-|:-|
|`Previous Link Label`|Text displayed on the link to the previous page.|
|`Next Link Label`|Text displayed on the link to the next page.|
|`First Link Label`|Text displayed on the link to the first page. If the text is empty, the link is hidden.|
|`Last Link Label`|Text displayed on the link to the last page. If the text is empty, the link is hidden.|

#### Rendering Parameters
- Path: /sitecore/templates/Feature/ESearch/Page Selector/Rendering Parameter/Page Selector Parameters

|Parameter name|Description|
|:-|:-|
|`Selector Size`|Number of the page selector links displayed on both sides of a center link.|

### Facet Filter
The Facet Filter component shows the number of search results in a specific field.

![](img/Component_FacetFilter.png)

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Facet Filter/Facet Filter

|Field name|Description|
|:-|:-|
|`Header Label`|Text displayed on the header of the filter.|
|`Clear Label`|Text displayed on the clear button.|

#### Rendering Parameters
- Path: /sitecore/templates/Feature/ESearch/Facet Filter/Rendering Parameter/Facet Filter Parameters

|Parameter name|Description|
|:-|:-|
|`Target Field`|Field for calculating the number of search results.|

### Sort Indicator
---

**NOTE**

This component uses the dropdown component of Bootstrap. If you don't want to use Bootstrap, you need to create a script for displaying it as a dropdown. (Would be improved in a feature version.)

---

The Sort Indicator has two dropdown lists for sorting the search results.

![](img/Component_SortIndicator.png)

The first one shows the field choices which is for sorting specified in the `Sort Fields` of the data source. The second one has "Ascending" or "Descending" choices and it is for specifying sort direction.  

To add a field choice:

1. Create a `Sort Field` item.
1. Input a field name for sorting in the `Field Name`.
1. Input a displayed label for the dropdown in the `Display Name`.
1. Set the item in the `Sort Fields` of the data source.

- Path: /sitecore/templates/Feature/ESearch/Sort Indicator/Sort Field

|Field name|Description|
|:-|:-|
|`Field Name`|Name of the field of the search target content.|
|`Display Name`|Name of display on dropdown list.|

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Sort Indicator/Sort Indicator

|Field name|Description|
|:-|:-|
|`Sort Fields`|`Sort Field` items to display on the dropdown list.|
|`Default Text`|Text to display on the initial choice in the dropdown list.|

#### Rendering Parameters
No rendering parameters in this component.

### Search Box
The Search Box is a component that helps the keyword search and provides suggestions.  

![](img/Component_SearchBox.png)

You can search for the field that was set in `Keyword Search Target` in` Search Settings`.

---

**NOTE**  

To enable typeahead suggestion, requierd to load `/Scripts/esearch.searchbox.js` in your layout.

---

#### Datasource Template
- Path: /sitecore/templates/Feature/ESearch/Search Box/Search Box

|Field name|Description|
|:-|:-|
|`Placeholder Label`|Text for display in the placeholder.|
|`Execute Button Label`|Text for display on the button that executes the search.|

#### Rendering Parameters
No rendering parameters in this component.

## Todos
The features below are not supported yet.

- [ ] Asynchronous update of the search results
- [ ] A range-based filter (ex: filter by price, date, etc.)
- [ ] Adding the components beyond a search page
- [ ] SXA Integrations

Your contribution and feedback are very welcomed.

- Make a feature in todo list
- Add unit testing
- Fix English in the documents or comments
- Add a sample theme without the Bootstrap
- Add a compatible rendering for each component

## For Customization

1. Clone this repository.
1. Build the solution and publish all projects to your Sitecore instance.
1. Enable the `/App_Config/Include/ESearch.Foundation.Serialization.config.example`. (published one, not in the repository)
1. In the enabled file, change the value of `sourceFolder` setting to the cloned solution's `/src` folder path.
1. Set the access (read and write) rights to an application pool of your Sitecore.
1. Access to `{url to your sitecore}/unicorn.aspx` and sync all items.

## License
This software is released under the MIT License, see [LICENSE.txt](LICENSE.txt).

## Authors
- [Takumi Yamada](https://github.com/xirtardauq)
- [Yuta Tsunemoto](https://github.com/yuta-tsunemoto)
- [Ayane Suzuki](https://github.com/ayane-suzuki)