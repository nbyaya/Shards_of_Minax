using System;
using Server;
using System.Xml;
using Server.Mobiles;

namespace Server.Regions
{
    public class DungeonRegion : BaseRegion
    {
        private Point3D m_EntranceLocation;
        private Map m_EntranceMap;

        public static readonly Point3D[] StableLocs = new Point3D[] { new Point3D(1510, 1543, 25), 
            new Point3D(1516, 1542, 25), new Point3D(1520, 1542, 25), new Point3D(1525, 1542, 25) };

        public DungeonRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
            XmlElement entrEl = xml["entrance"];

            Map entrMap = map;
            ReadMap(entrEl, "map", ref entrMap, false);

            if (ReadPoint3D(entrEl, entrMap, ref this.m_EntranceLocation, false))
                this.m_EntranceMap = entrMap;
        }

        public override void OnLocationChanged(Mobile m, Point3D oldLocation)
        {
            base.OnLocationChanged(m, oldLocation);

            if (m.AccessLevel > AccessLevel.Player)
                return;

            if (m.Mount != null)
            {
                if (m is PlayerMobile)
                {
                    (m as PlayerMobile).SetMountBlock(BlockMountType.DismountRecovery, TimeSpan.FromSeconds(30), true);
                }
                else
                {
                    m.Mount.Rider = null;
                }
		
                m.SendLocalizedMessage(1153052); // Mounts and flying are not permitted in this area.

                if (m.Mount is BaseCreature && ((BaseCreature)m.Mount).Controlled)
                {
                    BaseCreature mount = m.Mount as BaseCreature;
                    TryAutoStable(mount);
                }
            }

            //if (m is BaseCreature && ((BaseCreature)m).Controlled)
            //    TryAutoStable((BaseCreature)m);
        }

        public void TryAutoStable(BaseCreature pet)
        {
            if (pet == null)
                return;

            Mobile owner = pet.GetMaster();

            if (!pet.Controlled || owner == null)
            {
                return;
            }
            if (pet.Body.IsHuman || pet.IsDeadPet || pet.Allured)
            {
                SendToStables(pet, owner);
            }
            else if (owner.Stabled.Count >= AnimalTrainer.GetMaxStabled(owner))
            {
                SendToStables(pet, owner);
            }
            else if ((pet is PackLlama || pet is PackHorse || pet is Beetle) &&
                     (pet.Backpack != null && pet.Backpack.Items.Count > 0))
            {
                SendToStables(pet, owner);
            }
            else
            {
                pet.ControlTarget = null;
                pet.ControlOrder = OrderType.Stay;
                pet.Internalize();

                pet.SetControlMaster(null);
                pet.SummonMaster = null;

                pet.IsStabled = true;
                pet.StabledBy = owner;

                if (Core.SE)
                {
                    pet.Loyalty = AnimalTrainer.MaxLoyalty; // Wonderfully happy
                }

                owner.Stabled.Add(pet);
                //owner.SendLocalizedMessage(1153050, pet.Name); // Pets are not permitted in this location. Your pet named ~1_NAME~ has been sent to the stables.
            }
        }

        public void SendToStables(BaseCreature bc, Mobile m = null)
        {
            Point3D p = StableLocs[Utility.Random(StableLocs.Length)];
            bc.MoveToWorld(p, this.Map);

            if(m != null)
                m.SendLocalizedMessage(1153053, bc.Name); // Pets are not permitted in this area. Your pet named ~1_NAME~ could not be sent to the stables, so has been teleported outside the event area.
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override bool YoungProtected
        {
            get
            {
                return false;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D EntranceLocation
        {
            get
            {
                return this.m_EntranceLocation;
            }
            set
            {
                this.m_EntranceLocation = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map EntranceMap
        {
            get
            {
                return this.m_EntranceMap;
            }
            set
            {
                this.m_EntranceMap = value;
            }
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.DungeonLevel;
        }
    }
}