using HapGp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.ModelInstance
{
    public class ClassInstance:ProjectModel
    {
        ProjectModel proj;
        ProjectSelectModel projselect;
        LeaveModel leave;
        public ClassInstance(ProjectModel var1, ProjectSelectModel var2, LeaveModel var3)
        {
            proj = var1;
            projselect = var2;
            leave = var3;
        }

        public ProjectModel Proj { get => proj; set => proj = value; }
        public ProjectSelectModel Projselect { get => projselect; set => projselect = value; }
        public LeaveModel Leave { get => leave; set => leave = value; }
    }
}
