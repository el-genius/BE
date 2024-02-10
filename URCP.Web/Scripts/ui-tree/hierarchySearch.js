(function(){
    angular.module('demoApp.directives',[])
    .directive('hierarchySearch',function(HierarchyNodeService,$timeout) {
    
    return {
        restrict:'E',
        templateUrl: 'src/hierarchySearch.tpl.html',
        scope: {
            dataset:'='
        },
        controller:function($scope) {
            $scope.numSelected = 0;
            //$scope.list is used by ng-tree, dataset should never be modified
            $scope.list = angular.copy($scope.dataset);
            
            $scope.options = {};
            
            $scope.expandNode = function(n,$event) {
                $event.stopPropagation();
                n.toggle();
            }
            
            
            $scope.itemSelect = function(item) {
                var rootVal = !item.isSelected;
                HierarchyNodeService.selectChildren(item,rootVal)
                
                HierarchyNodeService.findParent($scope.list[0],null,item,selectParent);
                var s = _.compact(HierarchyNodeService.getAllChildren($scope.list[0],[]).map(function(c){ return c.isSelected && !c.items;}));
                $scope.numSelected = s.length;
            }   
            
            function selectParent(parent) {
                var children = HierarchyNodeService.getAllChildren(parent,[]);
                
                if(!children) return;
                children = children.slice(1).map(function(c){ return c.isSelected;});
                
                parent.isSelected =  children.length === _.compact(children).length;
                HierarchyNodeService.findParent($scope.list[0],null,parent,selectParent)
            }       

            $scope.nodeStatus = function(node) {
                var flattenedTree = getAllChildren(node,[]);
                flattenedTree = flattenedTree.map(function(n){ return n.isSelected });
    
                return flattenedTree.length === _.compact(flattenedTree);
            }   
        }
    }
})
}).call(this);