using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.MiniChamps;
using Server.Custom;

namespace Server.Items
{
    #region → MapModifier & Registry

    public class MapModifier
    {
        public string ID { get; }
        public string Name { get; }
        public string Description { get; }
        public Action<MagicMapBase, MagicMapBase.SpawnedContent> Apply { get; }

        public MapModifier(string id, string name, string description, Action<MagicMapBase, MagicMapBase.SpawnedContent> apply)
        {
            ID          = id;
            Name        = name;
            Description = description;
            Apply       = apply;
        }
    }

    public static class MapModifierRegistry
    {
		// inside MapModifierRegistry.All = new List<MapModifier> { … }
		#region → XML‐AOS Attribute Modifiers

		// Helper to DRY up the boilerplate
		static void AttachAttr<T>(
			MagicMapBase map,
			MagicMapBase.SpawnedContent content,
			Func<T>  createAttachment,
			Action<T> configure)
			where T : XmlAttachment
		{
			void AttachToCreature(BaseCreature bc)
			{
				if (bc == null || bc.Deleted) return;

				var existing = XmlAttach.FindAttachment(bc, typeof(T)) as T;
				if (existing == null)
				{
					var att = createAttachment();
					configure(att);
					XmlAttach.AttachTo(bc, att);
				}
				else
				{
					configure(existing);
				}
			}

			// attach to those already spawned
			foreach (var bc in content.SpawnedEntities.OfType<BaseCreature>())
				AttachToCreature(bc);

			// catch future spawns via MiniChamps
			foreach (var champ in content.SpawnedEntities.OfType<MiniChamp>())
				champ.CreatureSpawned += AttachToCreature;
		}        
		
		public static readonly List<MapModifier> All = new List<MapModifier>
        {
			// MapModifierRegistry.cs  (add to the list) ─────────────────────
			new MapModifier(
				"ArcaneAugment",          // internal ID
				"Arcane Augment",         // friendly name
				"Every creature here is born with an unpredictable magical power.",
				(map, content) =>
				{
					// ❶ – little helper that slaps the attachment on a creature
					void AttachAbility(BaseCreature bc)
					{
						if (bc == null || bc.Deleted) return;

						// Prevent double–attaching if the same creature is reported twice
						if (XmlAttach.FindAttachment(bc, typeof(XmlRandomAbility)) == null)
						{
							XmlAttach.AttachTo(bc, new XmlRandomAbility());
						}
					}

					// ❷ – creatures that have ALREADY spawned in this instance
					foreach (var bc in content.SpawnedEntities.OfType<BaseCreature>())
						AttachAbility(bc);

					// ❸ – subscribe to every Mini-Champ controller so we catch FUTURE spawns
					foreach (var champ in content.SpawnedEntities.OfType<MiniChamp>())
						champ.CreatureSpawned += AttachAbility;
				}
			),
            
			
			new MapModifier(
                "Firestorm",
                "Firestorm",
                "A ring of fire elementals emerges around the portal.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 6; i++)
                    {
                        var ang = i * (Math.PI*2/6);
                        var pt  = new Point3D(
                            centre.X + (int)(Math.Cos(ang)*map.SpawnRadius),
                            centre.Y + (int)(Math.Sin(ang)*map.SpawnRadius),
                            centre.Z);
                        var fe = new FireElemental();
                        fe.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(fe);
                    }
                }
            ),

            new MapModifier(
                "IceGale",
                "Ice Gale",
                "Slippery ice elementals drift in from the north.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 4; i++)
                    {
                        var pt = new Point3D(
                            centre.X,
                            centre.Y - map.SpawnRadius + (i*5),
                            centre.Z);
                        var ice = new WaterElemental();
                        ice.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(ice);
                    }
                }
            ),

            new MapModifier(
                "Earthquake",
                "Earthquake",
                "Rumbling earth elementals burst forth.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 5; i++)
                    {
                        var pt = new Point3D(
                            centre.X + Utility.RandomMinMax(-map.SpawnRadius, map.SpawnRadius),
                            centre.Y + Utility.RandomMinMax(-map.SpawnRadius, map.SpawnRadius),
                            centre.Z);
                        var ee = new EarthElemental();
                        ee.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(ee);
                    }
                }
            ),

            new MapModifier(
                "PoisonMist",
                "Poison Mist",
                "A choking mist spawns venomous spiders.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 8; i++)
                    {
                        var pt = new Point3D(
                            centre.X + Utility.RandomMinMax(-map.SpawnRadius/2, map.SpawnRadius/2),
                            centre.Y + Utility.RandomMinMax(-map.SpawnRadius/2, map.SpawnRadius/2),
                            centre.Z);
                        var sp = new GiantSpider();
                        sp.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(sp);
                    }
                }
            ),

            new MapModifier(
                "WolfPack",
                "Wolf Pack",
                "Hungry wolves prowl the perimeter.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 6; i++)
                    {
                        var pt   = map.GetValidMobileSpawnPoint(centre, facet, map.SpawnRadius);
                        var wolf = new DireWolf();
                        wolf.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(wolf);
                    }
                }
            ),

            new MapModifier(
                "SkeletonHorde",
                "Skeleton Horde",
                "Rattling skeleton warriors claw their way out.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 10; i++)
                    {
                        var pt = map.GetValidMobileSpawnPoint(centre, facet, map.SpawnRadius);
                        var sk = new SkeletalKnight();
                        sk.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(sk);
                    }
                }
            ),

            new MapModifier(
                "GoldRush",
                "Gold Rush",
                "Piles of gold coins spill across the ground.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 5; i++)
                    {
                        var pile = new Gold(Utility.RandomMinMax(200, 500));
                        var pt   = map.GetValidItemSpawnPoint(pile, centre, facet, map.SpawnRadius);
                        pile.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(pile);
                    }
                }
            ),

            new MapModifier(
                "MushroomBloom",
                "Mushroom Bloom",
                "Strange mushrooms sprout wildly.",
                (map, content) =>
                {
                    var centre = map.GetRandomLocation();
                    var facet  = map.DestinationFacet;
                    for (int i = 0; i < 12; i++)
                    {
                        var mush = new MagicMushroom();
                        var pt   = map.GetValidItemSpawnPoint(mush, centre, facet, map.SpawnRadius);
                        mush.MoveToWorld(pt, facet);
                        content.SpawnedEntities.Add(mush);
                    }
                }
            ),
			




			// XmlAosAttributes
			new MapModifier("RegenHits", "Regen Hits", "Creatures regenerate hit points.",        (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.RegenHits       = Utility.RandomMinMax(1,5))),
			new MapModifier("RegenStam", "Regen Stamina", "Creatures regenerate stamina.",       (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.RegenStam       = Utility.RandomMinMax(5,10))),
			new MapModifier("RegenMana", "Regen Mana", "Creatures regenerate mana.",             (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.RegenMana       = Utility.RandomMinMax(3,8))),
			new MapModifier("DefendChance", "Defend Chance", "Creatures get bonus defense.",     (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.DefendChance    = Utility.RandomMinMax(5,15))),
			new MapModifier("AttackChance", "Attack Chance", "Creatures get bonus attack.",      (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.AttackChance    = Utility.RandomMinMax(5,15))),
			new MapModifier("BonusStr", "Bonus Strength", "Creatures gain Strength.",            (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusStr       = Utility.RandomMinMax(10,30))),
			new MapModifier("BonusDex", "Bonus Dexterity", "Creatures gain Dexterity.",         (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusDex       = Utility.RandomMinMax(10,30))),
			new MapModifier("BonusInt", "Bonus Intelligence", "Creatures gain Intelligence.",   (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusInt       = Utility.RandomMinMax(10,30))),
			new MapModifier("BonusHits", "Bonus Hits", "Creatures gain extra hit points.",     (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusHits      = Utility.RandomMinMax(20,50))),
			new MapModifier("BonusStam", "Bonus Stamina", "Creatures gain extra stamina.",     (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusStam      = Utility.RandomMinMax(20,50))),
			new MapModifier("BonusMana", "Bonus Mana", "Creatures gain extra mana.",           (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.BonusMana      = Utility.RandomMinMax(20,50))),
			new MapModifier("WeaponDamage", "Weapon Damage", "Creatures hit harder.",          (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.WeaponDamage  = Utility.RandomMinMax(10,25))),
			new MapModifier("WeaponSpeed", "Weapon Speed", "Creatures attack faster.",         (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.WeaponSpeed   = Utility.RandomMinMax(5,15))),
			new MapModifier("SpellDamage", "Spell Damage", "Creatures’ spells hit harder.",    (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.SpellDamage   = Utility.RandomMinMax(10,25))),
			new MapModifier("CastRecovery", "Cast Recovery", "Creatures recover faster.",       (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.CastRecovery  = Utility.RandomMinMax(5,15))),
			new MapModifier("CastSpeed", "Cast Speed", "Creatures cast spells quicker.",      (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.CastSpeed     = Utility.RandomMinMax(5,15))),
			new MapModifier("LowerManaCost", "Lower Mana Cost", "Spells cost less mana.",     (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.LowerManaCost = Utility.RandomMinMax(5,10))),
			new MapModifier("LowerRegCost", "Lower Reagent Cost", "Spells use fewer reagents.",(map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.LowerRegCost = Utility.RandomMinMax(1,3))),
			new MapModifier("ReflectPhysical", "Reflect Physical", "Creatures reflect physical.",(map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.ReflectPhysical = Utility.RandomMinMax(5,20))),
			new MapModifier("EnhancePotions", "Enhance Potions", "Potions work stronger.",    (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.EnhancePotions = Utility.RandomMinMax(5,20))),
			new MapModifier("Luck", "Luck", "Creatures gain Luck.",                          (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.Luck           = Utility.RandomMinMax(50,100))),
			new MapModifier("SpellChanneling", "Spell Channeling", "Creatures can channel.",   (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.SpellChanneling = 1)),
			new MapModifier("NightSight", "Night Sight", "Creatures see in the dark.",        (map, content) => AttachAttr(map, content, () => new XmlAosAttributes(), att => att.NightSight     = 1)),

			// XmlAosWeaponAttributes
			new MapModifier("LowerStatReqWpn", "Lower Stat Req (Weapon)", "Wpn users need less stats.",       
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.LowerStatReq    = Utility.RandomMinMax(5,15))),
			new MapModifier("SelfRepairWpn", "Self Repair (Weapon)", "Wpn slowly repairs itself.",             
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.SelfRepair       = Utility.RandomMinMax(1,5))),
			new MapModifier("HitLeechHits", "Hit Leech Hits", "Hits steal hit points.",              
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLeechHits     = Utility.RandomMinMax(1,5))),
			new MapModifier("HitLeechStam", "Hit Leech Stamina", "Hits steal stamina.",            
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLeechStam     = Utility.RandomMinMax(1,5))),
			new MapModifier("HitLeechMana", "Hit Leech Mana", "Hits steal mana.",                  
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLeechMana     = Utility.RandomMinMax(1,5))),
			new MapModifier("HitLowerAttack", "Hit Lower Attack", "Hits lower target attack.",     
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLowerAttack   = Utility.RandomMinMax(5,15))),
			new MapModifier("HitLowerDefend", "Hit Lower Defense", "Hits lower target defense.",  
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLowerDefend   = Utility.RandomMinMax(5,15))),
			new MapModifier("HitMagicArrow", "Hit Magic Arrow", "Hits fire Magic Arrow.",         
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitMagicArrow    = Utility.RandomMinMax(1,3))),
			new MapModifier("HitHarm", "Hit Harm", "Hits cast Harm spell.",                    
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitHarm          = Utility.RandomMinMax(1,3))),
			new MapModifier("HitFireball", "Hit Fireball", "Hits cast Fireball.",               
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitFireball      = Utility.RandomMinMax(1,3))),
			new MapModifier("HitLightning", "Hit Lightning", "Hits cast Lightning.",            
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitLightning     = Utility.RandomMinMax(1,3))),
			new MapModifier("HitDispel", "Hit Dispel", "Hits cast Dispel.",                      
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitDispel        = Utility.RandomMinMax(1,2))),
			new MapModifier("HitColdArea", "Hit Cold Area", "Hits freeze around target.",       
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitColdArea      = Utility.RandomMinMax(1,3))),
			new MapModifier("HitFireArea", "Hit Fire Area", "Hits burn around target.",         
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitFireArea      = Utility.RandomMinMax(1,3))),
			new MapModifier("HitPoisonArea", "Hit Poison Area", "Hits poison around target.",    
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitPoisonArea    = Utility.RandomMinMax(1,3))),
			new MapModifier("HitEnergyArea", "Hit Energy Area", "Hits shock around target.",    
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitEnergyArea    = Utility.RandomMinMax(1,3))),
			new MapModifier("HitPhysicalArea", "Hit Physical Area", "Hits stagger around target.", 
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.HitPhysicalArea  = Utility.RandomMinMax(1,3))),
			new MapModifier("ResistPhysicalBonus", "Bonus Physical Resist", "Wpn wearer gets bonus Physical Resist.", 
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.ResistPhysicalBonus = Utility.RandomMinMax(5,20))),
			new MapModifier("ResistFireBonus",   "Bonus Fire Resist",    "Wpn wearer gets bonus Fire Resist.",    
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.ResistFireBonus     = Utility.RandomMinMax(5,20))),
			new MapModifier("ResistColdBonus",   "Bonus Cold Resist",    "Wpn wearer gets bonus Cold Resist.",    
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.ResistColdBonus     = Utility.RandomMinMax(5,20))),
			new MapModifier("ResistPoisonBonus", "Bonus Poison Resist",  "Wpn wearer gets bonus Poison Resist.",  
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.ResistPoisonBonus   = Utility.RandomMinMax(5,20))),
			new MapModifier("ResistEnergyBonus", "Bonus Energy Resist",  "Wpn wearer gets bonus Energy Resist.",  
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.ResistEnergyBonus   = Utility.RandomMinMax(5,20))),
			new MapModifier("UseBestSkill",     "Use Best Skill",       "Weapon always uses wearer’s best skill.", 
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.UseBestSkill       = 1)),
			new MapModifier("MageWeapon",       "Mage Weapon",          "Weapon is mage‐friendly.",              
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.MageWeapon         = 1)),
			new MapModifier("DurabilityBonusWpn","Durability Bonus",     "Weapon lasts longer.",                 
				(map, content) => AttachAttr(map, content, () => new XmlAosWeaponAttributes(), att => att.DurabilityBonus     = Utility.RandomMinMax(10,30))),

			// XmlAosArmorAttributes
			new MapModifier("LowerStatReqArm",  "Lower Stat Req (Armor)", "Armor needs less stats.",        
				(map, content) => AttachAttr(map, content, () => new XmlAosArmorAttributes(), att => att.LowerStatReq    = Utility.RandomMinMax(5,15))),
			new MapModifier("SelfRepairArm",    "Self Repair (Armor)",  "Armor slowly repairs itself.",      
				(map, content) => AttachAttr(map, content, () => new XmlAosArmorAttributes(), att => att.SelfRepair       = Utility.RandomMinMax(1,5))),
			new MapModifier("MageArmor",        "Mage Armor",           "Armor is mage‐friendly.",            
				(map, content) => AttachAttr(map, content, () => new XmlAosArmorAttributes(), att => att.MageArmor        = 1)),
			new MapModifier("DurabilityBonusArm","Durability Bonus",     "Armor lasts longer.",               
				(map, content) => AttachAttr(map, content, () => new XmlAosArmorAttributes(), att => att.DurabilityBonus  = Utility.RandomMinMax(10,30))),

			// XmlAosElementAttributes
			new MapModifier("ElemPhys",  "Elemental: Physical", "Adds Physical damage.",  (map, content) => AttachAttr(map, content, () => new XmlAosElementAttributes(), att => att.Physical = Utility.RandomMinMax(5,20))),
			new MapModifier("ElemFire",  "Elemental: Fire",     "Adds Fire damage.",      (map, content) => AttachAttr(map, content, () => new XmlAosElementAttributes(), att => att.Fire     = Utility.RandomMinMax(5,20))),
			new MapModifier("ElemCold",  "Elemental: Cold",     "Adds Cold damage.",      (map, content) => AttachAttr(map, content, () => new XmlAosElementAttributes(), att => att.Cold     = Utility.RandomMinMax(5,20))),
			new MapModifier("ElemPoison","Elemental: Poison",   "Adds Poison damage.",    (map, content) => AttachAttr(map, content, () => new XmlAosElementAttributes(), att => att.Poison   = Utility.RandomMinMax(5,20))),
			new MapModifier("ElemEnergy","Elemental: Energy",   "Adds Energy damage.",    (map, content) => AttachAttr(map, content, () => new XmlAosElementAttributes(), att => att.Energy   = Utility.RandomMinMax(5,20))),

			#endregion

			new MapModifier("FieryAura", "Fiery Aura", "Creatures erupt in flame—burning attackers and nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlFire(Utility.RandomMinMax(5, 20), Utility.RandomMinMax(3, 8)), att => att.Range = Utility.RandomMinMax(3, 8))),
			new MapModifier("RandomHue", "Random Hue", "Creatures shimmer with unpredictable magical colors.", (map, content) => AttachAttr(map, content, () => new XmlHue(Utility.RandomMinMax(2, 3001)), att => {})),
			new MapModifier("LifeDrain", "Life Drain", "Creatures sap life from enemies on hit and proximity.", (map, content) => AttachAttr(map, content, () => new XmlLifeDrain(Utility.RandomMinMax(5, 15), Utility.RandomMinMax(3, 7)), att => att.Range = Utility.RandomMinMax(3, 6))),
			new MapModifier("LightningAura", "Lightning Aura", "Creatures are charged with lightning, shocking attackers and nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlLightning(Utility.RandomMinMax(5, 20), Utility.RandomMinMax(3, 8)), att => att.Range = Utility.RandomMinMax(3, 8))),
			new MapModifier("ManaDrain", "Mana Drain", "Creatures sap mana from nearby foes and on every strike.", (map, content) => AttachAttr(map, content, () => new XmlManaDrain(Utility.RandomMinMax(5, 15), Utility.RandomMinMax(3, 8)), att => att.Range = Utility.RandomMinMax(3, 8))),
			new MapModifier("OgreMorph", "Ogre Morph", "Every creature takes on the brutish form of an ogre.", (map, content) => AttachAttr(map, content, () => new XmlMorph(1, 999.5), att => att.Range = 2)),
			new MapModifier("RandomMorph", "Random Morph", "Every creature morphs into a bizarre new form.", (map, content) => AttachAttr(map, content, () => new XmlMorph(Utility.RandomMinMax(1, 319), 999.5), att => att.Range = 2)),
			new MapModifier("PoisonLesser",  "Lesser Poison",  "Creatures poison with a weak toxin.",     (map, content) => AttachAttr(map, content, () => new XmlPoison(0), _ => {})),
			new MapModifier("PoisonRegular", "Regular Poison", "Creatures poison with a standard toxin.",  (map, content) => AttachAttr(map, content, () => new XmlPoison(1), _ => {})),
			new MapModifier("PoisonGreater", "Greater Poison", "Creatures poison with a potent toxin.",    (map, content) => AttachAttr(map, content, () => new XmlPoison(2), _ => {})),
			new MapModifier("PoisonDeadly",  "Deadly Poison",  "Creatures poison with a deadly toxin.",    (map, content) => AttachAttr(map, content, () => new XmlPoison(3), _ => {})),
			new MapModifier("PoisonLethal",  "Lethal Poison",  "Creatures poison with a lethal toxin.",    (map, content) => AttachAttr(map, content, () => new XmlPoison(4), _ => {})),
			new MapModifier("WrestlingMastery", "Wrestling Mastery", "Creatures are expert grapplers, striking with unerring force.", (map, content) => AttachAttr(map, content, () => new XmlSkill("WrestlingMastery", "Wrestling", 100), att => {})),
			new MapModifier("StaminaDrain", "Stamina Drain", "Creatures sap stamina with every hit or step too close.", (map, content) => AttachAttr(map, content, () => new XmlStamDrain(Utility.RandomMinMax(5, 15), Utility.RandomMinMax(3, 8)), att => att.Range = Utility.RandomMinMax(3, 8))),
			// XmlWeaponAbility modifiers (one‐liners)
			new MapModifier("ArmorIgnore",        "Armor Ignore",        "Creatures’ hits ignore part of the target’s armor.",           (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ArmorIgnore"),        att => { })),
			new MapModifier("BleedAttack",        "Bleed Attack",        "Creatures’ hits cause bleeding damage over time.",             (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("BleedAttack"),        att => { })),
			new MapModifier("ConcussionBlow",     "Concussion Blow",     "Creatures’ hits daze their foes.",                             (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ConcussionBlow"),    att => { })),
			new MapModifier("CrushingBlow",       "Crushing Blow",       "Creatures’ hits deal bonus crushing damage.",                  (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("CrushingBlow"),      att => { })),
			new MapModifier("Disarm",             "Disarm",              "Creatures’ hits attempt to disarm their foes.",                (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Disarm"),            att => { })),
			new MapModifier("Dismount",           "Dismount",            "Creatures’ hits knock riders off their mounts.",               (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Dismount"),          att => { })),
			new MapModifier("DoubleStrike",       "Double Strike",       "Creatures’ hits strike twice rapidly.",                        (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("DoubleStrike"),      att => { })),
			new MapModifier("InfectiousStrike",   "Infectious Strike",   "Creatures’ hits poison their foes.",                           (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("InfectiousStrike"),  att => { })),
			new MapModifier("MortalStrike",       "Mortal Strike",       "Creatures’ hits deal massive damage but cost mana.",           (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("MortalStrike"),      att => { })),
			new MapModifier("MovingShot",         "Moving Shot",         "Creatures fire while moving with no penalty.",                 (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("MovingShot"),        att => { })),
			new MapModifier("ParalyzingBlow",     "Paralyzing Blow",     "Creatures’ hits paralyze their foes.",                         (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ParalyzingBlow"),    att => { })),
			new MapModifier("ShadowStrike",       "Shadow Strike",       "Creatures’ hits hit from the shadows with bonus damage.",      (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ShadowStrike"),      att => { })),
			new MapModifier("WhirlwindAttack",    "Whirlwind Attack",    "Creatures spin in a whirlwind, damaging all around them.",     (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("WhirlwindAttack"),   att => { })),
			new MapModifier("RidingSwipe",        "Riding Swipe",        "Creatures hit while mounted deal extra damage.",               (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("RidingSwipe"),        att => { })),
			new MapModifier("FrenziedWhirlwind",  "Frenzied Whirlwind",  "Creatures’ whirlwind hits even harder in rage.",              (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("FrenziedWhirlwind"), att => { })),
			new MapModifier("Block",              "Block",               "Creatures occasionally block incoming attacks.",               (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Block"),              att => { })),
			new MapModifier("DefenseMastery",     "Defense Mastery",     "Creatures excel at parrying and blocking.",                    (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("DefenseMastery"),     att => { })),
			new MapModifier("NerveStrike",        "Nerve Strike",        "Creatures’ hits lower their foe’s combat effectiveness.",      (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("NerveStrike"),        att => { })),
			new MapModifier("TalonStrike",        "Talon Strike",        "Creatures’ hits tear at vital arteries.",                      (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("TalonStrike"),        att => { })),
			new MapModifier("Feint",              "Feint",               "Creatures’ hits feint to open defenses.",                      (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Feint"),              att => { })),
			new MapModifier("DualWield",          "Dual Wield",          "Creatures fight with two weapons simultaneously.",             (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("DualWield"),          att => { })),
			new MapModifier("DoubleShot",         "Double Shot",         "Creatures fire two projectiles in quick succession.",          (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("DoubleShot"),         att => { })),
			new MapModifier("ArmorPierce",        "Armor Pierce",        "Creatures’ hits pierce through armor.",                        (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ArmorPierce"),        att => { })),
			new MapModifier("Bladeweave",         "Bladeweave",          "Creatures’ weapon strikes weave between attacks.",             (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Bladeweave"),         att => { })),
			new MapModifier("ForceArrow",         "Force Arrow",         "Creatures’ arrows hit like force bolts.",                      (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ForceArrow"),         att => { })),
			new MapModifier("LightningArrow",     "Lightning Arrow",     "Creatures’ arrows strike with lightning.",                     (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("LightningArrow"),     att => { })),
			new MapModifier("PsychicAttack",      "Psychic Attack",      "Creatures’ hits deal psychic damage.",                         (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("PsychicAttack"),      att => { })),
			new MapModifier("SerpentArrow",       "Serpent Arrow",       "Creatures’ arrows summon venomous serpents.",                  (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("SerpentArrow"),       att => { })),
			new MapModifier("ForceOfNature",      "Force of Nature",     "Creatures’ throws unleash nature’s wrath.",                    (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ForceOfNature"),      att => { })),
			new MapModifier("InfusedThrow",       "Infused Throw",       "Creatures’ thrown weapons carry elemental energy.",            (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("InfusedThrow"),       att => { })),
			new MapModifier("MysticArc",          "Mystic Arc",          "Creatures’ hits create a mystic arc of energy.",              (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("MysticArc"),          att => { })),
			new MapModifier("Disrobe",            "Disrobe",             "Creatures’ hits strip armor from their foes.",                 (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("Disrobe"),            att => { })),
			new MapModifier("ColdWind",           "Cold Wind",           "Creatures’ hits send a blast of freezing air.",                (map, content) => AttachAttr(map, content, () => new XmlWeaponAbility("ColdWind"),           att => { })),
			new MapModifier(
				"PhantomStrike", 
				"Phantom Strike", 
				"20% of creatures may suddenly phase behind and strike for extra damage.", 
				(map, content) => { if (Utility.RandomDouble() > 0.20) return; AttachAttr(map, content, () => new XmlPhantomStrike(), att => { }); }
			),
			new MapModifier(
			  "SilentGale",
			  "Silent Gale",
			  "Some creatures move with uncanny silence, briefly maxing their Stealth skill.",
			  (map, content) => {
				void A(BaseCreature bc) {
				  if (bc != null && !bc.Deleted && XmlAttach.FindAttachment(bc, typeof(XmlSilentGale)) == null && Utility.RandomDouble() <= 0.2) 
					XmlAttach.AttachTo(bc, new XmlSilentGale());
				}
				foreach (var bc in content.SpawnedEntities.OfType<BaseCreature>()) A(bc);
				foreach (var champ in content.SpawnedEntities.OfType<MiniChamp>()) champ.CreatureSpawned += A;
			  }
			),
			new MapModifier("AirborneEscape", "Airborne Escape", "Some creatures may vanish briefly in a sudden burst of agility.", (map, content) => AttachAttr(map, content, () => new XmlAirborneEscape(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Vanish", "Vanish", "Some creatures fade into invisibility mid‐combat.", (map, content) => AttachAttr(map, content, () => new XmlInvisibility(Utility.RandomMinMax(6,12), Utility.RandomMinMax(45,75)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GoldRain", "Gold Rain", "Some creatures shower attackers with bursts of gold.", (map, content) => AttachAttr(map, content, () => new XmlGoldRain(Utility.RandomMinMax(5,10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SavageStrike", "Savage Strike", "Some creatures strike with brutal, bleeding fury.", (map, content) => AttachAttr(map, content, () => new XmlSavageStrike(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RallyingRoar", "Rallying Roar", "Some creatures unleash a roar that heals nearby allies.", (map, content) => AttachAttr(map, content, () => new XmlRallyingRoar(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TrickDecoy", "Trick Decoy", "Some creatures create confusing decoys when attacked.", (map, content) => AttachAttr(map, content, () => new XmlTrickDecoy(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TrickstersGambit", "Trickster's Gambit", "Some creatures strike with sudden surges of strength and speed.", (map, content) => AttachAttr(map, content, () => new XmlTrickstersGambit(Utility.RandomMinMax(8, 15)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TrapLayer", "Trap Layer", "Some creatures set magical traps when they strike.", (map, content) => AttachAttr(map, content, () => new XmlTrap(Utility.RandomMinMax(4, 8)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FireBreath", "Fire Breath", "Some creatures unleash devastating fire breath cones.", (map, content) => AttachAttr(map, content, () => new XmlFireBreathAttack(Utility.RandomMinMax(30, 60), Utility.RandomMinMax(6, 10), 0.5), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BananaBomb", "Banana Bomb", "Some creatures toss explosive banana bombs when fighting.", (map, content) => AttachAttr(map, content, () => new XmlBananaBomb(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FrenziedAttack", "Frenzied Attack", "Some creatures unleash rapid strikes after landing a hit.", (map, content) => AttachAttr(map, content, () => new XmlFrenziedAttack(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RageFury", "Rage Fury", "Some creatures may erupt in furious rage, striking harder but with less defense.", (map, content) => AttachAttr(map, content, () => new XmlRage(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Blinkstep", "Blinkstep", "Some creatures randomly teleport when struck.", (map, content) => AttachAttr(map, content, () => new XmlTeleport(Utility.RandomMinMax(6, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SolarFlare", "Solar Flare", "Some creatures unleash a blinding burst of sunlight.", (map, content) => AttachAttr(map, content, () => new XmlSolarFlare(Utility.RandomMinMax(15, 30), Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SunlitHeal", "Sunlit Heal", "Some creatures radiate healing light after striking.", (map, content) => AttachAttr(map, content, () => new XmlSunlitHeal(Utility.RandomMinMax(20, 40), Utility.RandomMinMax(0.8, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RadiantShield", "Radiant Shield", "Some creatures gain bursts of magical defense.", (map, content) => AttachAttr(map, content, () => new XmlRadiantShield(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SolarBurst", "Solar Burst", "Some creatures unleash a blinding explosion of solar energy.", (map, content) => AttachAttr(map, content, () => new XmlSolarBurst(Utility.RandomMinMax(20, 40), Utility.RandomMinMax(2, 4)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MelodyOfPeace", "Melody of Peace", "Some creatures soothe allies with a healing tune.", (map, content) => AttachAttr(map, content, () => new XmlMelodyOfPeace(Utility.RandomMinMax(10, 20), Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("HarmonyEcho", "Harmony Echo", "Some creatures emit a stunning, musical shockwave.", (map, content) => AttachAttr(map, content, () => new XmlHarmonyEcho(Utility.RandomMinMax(40, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CrescendoOfJoy", "Crescendo of Joy", "Some creatures unleash a musical blast that freezes and harms nearby enemies.", (map, content) => AttachAttr(map, content, () => new XmlCrescendoOfJoy(Utility.RandomMinMax(10, 25), 4, 1), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ResonantAura", "Resonant Aura", "Some creatures emit a magical aura that empowers allies and hinders enemies.", (map, content) => AttachAttr(map, content, () => new XmlResonantAura(Utility.RandomMinMax(1, 3)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SparklingAura", "Sparkling Aura", "Some creatures emit a radiant pulse that burns nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlSparklingAura(Utility.RandomMinMax(3, 7), Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("DreamDust", "Dream Dust", "Some creatures scatter sleep-inducing powder that confuses and freezes.", (map, content) => AttachAttr(map, content, () => new XmlDreamDust(Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("DreamWeave", "Dream Weave", "Some creatures emit a healing aura that boosts nearby allies.", (map, content) => AttachAttr(map, content, () => new XmlDreamWeave(Utility.RandomMinMax(10, 20), 45, 45), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("DreamyAura", "Dreamy Aura", "Some creatures pulse with a dreamlike aura that disorients nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlDreamyAura(Utility.RandomMinMax(4, 8), Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("IllusionTrick", "Illusion Trick", "Some creatures spawn magical duplicates when struck.", (map, content) => AttachAttr(map, content, () => new XmlIllusionAbility(0.25, TimeSpan.FromMinutes(2)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SparkleBlast", "Sparkle Blast", "Some creatures unleash dazzling blasts that confuse and harm nearby players.", (map, content) => AttachAttr(map, content, () => new XmlSparkleBlast(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GlitterShield", "Glitter Shield", "Some creatures are shielded by shimmering magical armor.", (map, content) => AttachAttr(map, content, () => new XmlGlitterShield(10, 1, 20), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TeleportAbility", "Teleport Ability", "Some creatures vanish and reappear mid-fight.", (map, content) => AttachAttr(map, content, () => new XmlTeleportAbility(Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("StardustBeam", "Stardust Beam", "Some creatures unleash a stunning beam of stardust.", (map, content) => AttachAttr(map, content, () => new XmlStardustBeam(Utility.RandomMinMax(20, 40), 2.0, Utility.RandomMinMax(1.5, 3.0)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FrostyTrail", "Frosty Trail", "Some creatures leave behind a freezing trail of ice.", (map, content) => AttachAttr(map, content, () => new XmlFrostyTrail(Utility.RandomMinMax(25, 45)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("EvasiveManeuver", "Evasive Maneuver", "Some creatures dart and dodge with sudden bursts of speed.", (map, content) => AttachAttr(map, content, () => new XmlEvasiveManeuver(Utility.RandomMinMax(15, 30), Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("StaticShock", "Static Shock", "Some creatures unleash a stunning electric burst on hit.", (map, content) => AttachAttr(map, content, () => new XmlStaticShock(5, 15, 0.5, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("StarlightBurst", "Starlight Burst", "Some creatures unleash a radiant burst that blinds and burns nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlStarlightBurst(Utility.RandomMinMax(15, 30), 1), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GravityWarp", "Gravity Warp", "Some creatures distort gravity, damaging and disorienting nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlGravityWarp(Utility.RandomMinMax(10, 20), Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CosmicCloak", "Cosmic Cloak", "Some creatures vanish in shimmering starlight when struck.", (map, content) => AttachAttr(map, content, () => new XmlCosmicCloak(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("StarShower", "Star Shower", "Some creatures unleash dazzling starbursts on impact.", (map, content) => AttachAttr(map, content, () => new XmlStarShower(Utility.RandomMinMax(10, 20), 20), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BubbleBurst", "Bubble Burst", "Some creatures unleash chilling bubbles that knock back nearby enemies.", (map, content) => AttachAttr(map, content, () => new XmlBubbleBurst(Utility.RandomMinMax(10, 20)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ElectricalSurge", "Electrical Surge", "Some creatures surge with crackling power mid-battle.", (map, content) => AttachAttr(map, content, () => new XmlElectricalSurge(15, 10, 30, 20, 10), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FrostNova", "Frost Nova", "Some creatures erupt with a chilling blast that freezes nearby players.", (map, content) => AttachAttr(map, content, () => new XmlFrostNova(Utility.RandomMinMax(10, 20), Utility.RandomMinMax(3, 6), Utility.RandomMinMax(1, 3)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SoothingWind", "Soothing Wind", "Some creatures emit a calming wind, boosting allies’ speed.", (map, content) => AttachAttr(map, content, () => new XmlSoothingWind(Utility.RandomMinMax(10, 25), 30, 240), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GustBarrier", "Gust Barrier", "Some creatures occasionally shield allies with a gust of protective wind.", (map, content) => AttachAttr(map, content, () => new XmlGustBarrier(Utility.RandomMinMax(15, 30), Utility.RandomMinMax(0.5, 1.5), Utility.RandomMinMax(180, 240)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BreezeHeal", "Healing Breeze", "Some creatures release healing energy on strike, aiding nearby allies.", (map, content) => AttachAttr(map, content, () => new XmlBreezeHeal(10, 20, Utility.RandomMinMax(90, 150)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("StormDash", "Storm Dash", "Some creatures lunge forward, striking hard and stunning their target.", (map, content) => AttachAttr(map, content, () => new XmlStormDash(20, 30, 1.0, Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WhirlwindFury", "Whirlwind Fury", "Some creatures unleash a whirlwind that strikes nearby players.", (map, content) => AttachAttr(map, content, () => new XmlWhirlwindFury(15, 25, 5, Utility.RandomMinMax(25, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("VortexPull", "Vortex Pull", "Some creatures can pull nearby enemies into a damaging vortex.", (map, content) => AttachAttr(map, content, () => new XmlVortexPull(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TempestBreath", "Tempest Breath", "Some creatures unleash a knockback storm on hit.", (map, content) => AttachAttr(map, content, () => new XmlTempestBreath(20, 30, Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("LightningStrike", "Lightning Strike", "Some creatures unleash lightning when they strike.", (map, content) => AttachAttr(map, content, () => new XmlLightningStrike(30, 50, Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("HeavenlyStrike", "Heavenly Strike", "Some creatures channel divine wrath into their blows.", (map, content) => AttachAttr(map, content, () => new XmlHeavenlyStrike(Utility.RandomMinMax(30, 50), Utility.RandomMinMax(10, 25), Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CycloneCharge", "Cyclone Charge", "Some creatures teleport-charge enemies with violent force.", (map, content) => AttachAttr(map, content, () => new XmlCycloneCharge(Utility.RandomMinMax(15, 35), Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MistyStep", "Misty Step", "Some creatures teleport away when struck.", (map, content) => AttachAttr(map, content, () => new XmlMistyStep(Utility.RandomMinMax(6, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CycloneRampage", "Cyclone Rampage", "Some creatures unleash a violent spinning rampage.", (map, content) => AttachAttr(map, content, () => new XmlCycloneRampage(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WindBlast", "Wind Blast", "Some creatures unleash a gust that damages and knocks back nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlWindBlast(10, 20, 3, Utility.RandomMinMax(3, 6)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FossilBurst", "Fossil Burst", "Some creatures release a pulse of ancient damage when striking.", (map, content) => AttachAttr(map, content, () => new XmlFossilBurst(20, 30, Utility.RandomMinMax(25, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GroundSlam", "Ground Slam", "Some creatures slam the ground, damaging and knocking back nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlGroundSlam(20, 30, 4, Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PrismaticBurst", "Prismatic Burst", "Some creatures unleash a stunning burst of light when striking.", (map, content) => AttachAttr(map, content, () => new XmlPrismaticBurst(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("LavaWave", "Lava Wave", "Some creatures erupt with searing waves of lava when struck.", (map, content) => AttachAttr(map, content, () => new XmlLavaWave(10, 20, Utility.RandomMinMax(25, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("LavaFlow", "Lava Flow", "Some creatures erupt in molten lava, scorching all nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlLavaFlow(Utility.RandomMinMax(15, 35), Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MudBomb", "Mud Bomb", "Some creatures hurl mud globs that freeze and hurt attackers.", (map, content) => AttachAttr(map, content, () => new XmlMudBomb(Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MudTrap", "Mud Trap", "Some creatures mire attackers in sticky mud, slowing movement.", (map, content) => AttachAttr(map, content, () => new XmlMudTrap(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("EarthquakeStrike", "Earthquake Strike", "Some creatures unleash earthquakes that freeze and injure nearby players.", (map, content) => AttachAttr(map, content, () => new XmlEarthquake { Damage = Utility.RandomMinMax(20, 40), FreezeDuration = TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6)), MinCooldown = TimeSpan.FromSeconds(30), MaxCooldown = TimeSpan.FromSeconds(60) }, att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GraniteSlam", "Granite Slam", "Some creatures unleash a powerful ground slam that knocks foes back.", (map, content) => AttachAttr(map, content, () => new XmlGraniteSlam(50, 70, Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("VolcanicEruption", "Volcanic Eruption", "Some creatures erupt in molten lava, scorching nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlVolcanicEruption(Utility.RandomMinMax(20, 30)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ChaoticSurge", "Chaotic Surge", "Some creatures unleash random magical effects in combat.", (map, content) => AttachAttr(map, content, () => new XmlChaoticSurge(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WaterVortex", "Water Vortex", "Some creatures drag nearby players into a swirling vortex of water.", (map, content) => AttachAttr(map, content, () => new XmlWaterVortex(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RainOfWrath", "Rain of Wrath", "Some creatures unleash magical storms when striking foes.", (map, content) => AttachAttr(map, content, () => new XmlRainOfWrath(Utility.RandomMinMax(1.5, 3.0)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ToxicSludge", "Toxic Sludge", "Some creatures form poisonous sludge pools when they strike.", (map, content) => AttachAttr(map, content, () => new XmlToxicSludge(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FrostBreath", "Frost Breath", "Some creatures freeze and chill their victims with icy breath.", (map, content) => AttachAttr(map, content, () => new XmlFrostBreath(10, 15, 2.0, Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TideSurge", "Tide Surge", "Some creatures unleash a watery blast, pushing enemies back.", (map, content) => AttachAttr(map, content, () => new XmlTideSurge(Utility.RandomMinMax(5, 15), Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("AbyssalWave", "Abyssal Wave", "Some creatures unleash dark waves that damage and displace nearby players.", (map, content) => AttachAttr(map, content, () => new XmlAbyssalWave(15, 25, Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TidalPull", "Tidal Pull", "Some creatures can pull nearby players into melee range.", (map, content) => AttachAttr(map, content, () => new XmlTidalPull(Utility.RandomMinMax(30, 50)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BoilingSurge", "Boiling Surge", "Some creatures surge with boiling fury, briefly boosting their damage.", (map, content) => AttachAttr(map, content, () => new XmlBoilingSurge(Utility.RandomMinMax(10, 20), Utility.RandomMinMax(1.5, 3.0)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MinionStrike", "Minion Strike", "Some creatures summon random minions when attacking.", (map, content) => AttachAttr(map, content, () => new XmlRandomMinionStrike(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MoltenBlast", "Molten Blast", "Some creatures erupt with searing blasts of fire on attack.", (map, content) => AttachAttr(map, content, () => new XmlMoltenBlast(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("InfernoAura", "Inferno Aura", "Some creatures radiate searing heat, burning nearby players.", (map, content) => AttachAttr(map, content, () => new XmlInfernoAura(5, 10, 1.0), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BlazingCharge", "Blazing Charge", "Some creatures charge their foes in a burst of fire.", (map, content) => AttachAttr(map, content, () => new XmlBlazingCharge(Utility.RandomMinMax(45, 75)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("HellfireStorm", "Hellfire Storm", "Some creatures unleash a fiery storm upon striking!", (map, content) => AttachAttr(map, content, () => new XmlHellfireStorm(20, 40, Utility.RandomMinMax(1, 2)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FieryIllusion", "Fiery Illusion", "Some creatures spawn fiery mirages when struck.", (map, content) => AttachAttr(map, content, () => new XmlFieryIllusion(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BlazingTrail", "Blazing Trail", "Some creatures dash in flames, scorching those nearby.", (map, content) => AttachAttr(map, content, () => new XmlBlazingTrail(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ScorchedBite", "Scorched Bite", "Some creatures deliver a searing bite that burns and exhausts.", (map, content) => AttachAttr(map, content, () => new XmlScorchedBite(Utility.RandomMinMax(15, 30), Utility.RandomMinMax(10, 25), Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PhantomBurn", "Phantom Burn", "Some creatures inflict a searing phantom burn on their foes.", (map, content) => AttachAttr(map, content, () => new XmlPhantomBurn(Utility.RandomMinMax(8, 15), Utility.RandomMinMax(1.5, 3.0)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FlameCoil", "Flame Coil", "Some creatures unleash fiery coils that scorch and hinder foes.", (map, content) => AttachAttr(map, content, () => new XmlFlameCoil(Utility.RandomMinMax(10, 20), 5, Utility.RandomMinMax(30, 60)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FireWalk", "Fire Walk", "Some creatures blink away in flame, scorching nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlFireWalk(Utility.RandomMinMax(0.5, 1.5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Eruption", "Volcanic Eruption", "Some creatures trigger fiery eruptions on hit.", (map, content) => AttachAttr(map, content, () => new XmlEruption { Count = Utility.RandomMinMax(2, 4), MinDelay = TimeSpan.FromSeconds(90), MaxDelay = TimeSpan.FromSeconds(150) }, att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RandomAbility", "Random Ability", "Some creatures manifest chaotic magical abilities.", (map, content) => AttachAttr(map, content, () => new XmlRandomAbility(1, 3), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BackStrike", "Phantom Strike", "Some creatures blink behind and strike unexpectedly.", (map, content) => AttachAttr(map, content, () => new XmlBackStrike(Utility.RandomMinMax(20, 40)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BubbleShield", "Bubble Shield", "Some creatures are protected by a magical shield of icy bubbles.", (map, content) => AttachAttr(map, content, () => new XmlBubbleShield(10, 30), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TrueFear", "True Fear", "Some monsters strike such terror they freeze their foes.", (map, content) => AttachAttr(map, content, () => new XmlTrueFear(Utility.RandomMinMax(8, 15)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Disguise", "Shapeshifters", "Some creatures assume a new identity mid-fight.", (map, content) => AttachAttr(map, content, () => new XmlDisguise(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FireNuke", "Fire Nuke", "Some creatures unleash fiery explosions around them.", (map, content) => AttachAttr(map, content, () => new XmlNuke(Utility.RandomMinMax(40, 70), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GaleStrike", "Gale Strike", "Some creatures unleash a wind blast on hit, flinging enemies away.", (map, content) => AttachAttr(map, content, () => new XmlGaleStrike(15, 25, Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ArcaneExplosion", "Arcane Explosion", "Some creatures erupt in magical blasts on hit.", (map, content) => AttachAttr(map, content, () => new XmlArcaneExplosion(Utility.RandomMinMax(10, 30), Utility.RandomMinMax(8, 15)), att => { att.Range = Utility.RandomMinMax(2, 4); if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("AstralStrike", "Astral Strike", "Some creatures unleash sustained astral damage over time.", (map, content) => AttachAttr(map, content, () => new XmlAstralStrike(Utility.RandomMinMax(2, 6), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("AxeCircle", "Circle of Axe", "Some creatures unleash a axe ring attack when striking foes.", (map, content) => AttachAttr(map, content, () => new XmlAxeCircle(Utility.RandomMinMax(8, 12), 20, 5, 4), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Backstabber", "Backstabber", "Some creatures strike brutally from the shadows.", (map, content) => AttachAttr(map, content, () => new XmlBackstab(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BeeLine", "Bee Line", "Some creatures unleash a Bee line attack when striking.", (map, content) => AttachAttr(map, content, () => new XmlBeeLine(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(20, 40), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BladesBreath", "Blades Breath", "Some creatures unleash a blade-shaped breath attack.", (map, content) => AttachAttr(map, content, () => new XmlBladesBreath(Utility.RandomMinMax(1, 2), Utility.RandomMinMax(15, 30), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BladeStrike", "Blade Strike", "Some creatures unleash a whirling blade storm on hit.", (map, content) => AttachAttr(map, content, () => new XmlBladeStrike(Utility.RandomMinMax(3, 8), Utility.RandomMinMax(5, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BlastStrike", "Blast Strike", "Some creatures trigger a searing blast that burns over time.", (map, content) => AttachAttr(map, content, () => new XmlBlastStrike(Utility.RandomMinMax(5,10), Utility.RandomMinMax(5,10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BlazeStrike", "Blaze Strike", "Some creatures ignite foes in searing flame over time.", (map, content) => AttachAttr(map, content, () => new XmlBlazeStrike(Utility.RandomMinMax(5,10), Utility.RandomMinMax(5,10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BoulderBreath", "Boulder Breath", "Some creatures unleash a crushing cone of boulders.", (map, content) => AttachAttr(map, content, () => new XmlBoulderBreath(Utility.RandomMinMax(1, 2), Utility.RandomMinMax(20, 40), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("BreathAttack", "Breath Attack", "Some creatures unleash devastating breath attacks.", (map, content) => AttachAttr(map, content, () => new XmlBreathAttack(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(15, 30), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ChillTouch", "Chill Touch", "Some creatures strike with freezing blows that slow and damage.", (map, content) => AttachAttr(map, content, () => new XmlChillTouch(0.15, 20, 0.15, 15), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CursedTouch", "Cursed Touch", "Some creatures curse their victims to suffer more damage.", (map, content) => AttachAttr(map, content, () => new XmlCursedTouch(1.2, Utility.RandomMinMax(8, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("CorpseDevour", "Corpse Devour", "Some creatures can consume nearby corpses to heal.", (map, content) => AttachAttr(map, content, () => new XmlDevour(Utility.RandomMinMax(10, 25), Utility.RandomMinMax(0.5, 2.0)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Disarm", "Disarming Blow", "Some creatures may knock the weapon from your hands.", (map, content) => AttachAttr(map, content, () => new XmlDisarm(Utility.RandomMinMax(8, 15)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Disarmor", "Disarmor", "Some creatures can knock armor off their victims.", (map, content) => AttachAttr(map, content, () => new XmlDisarmor(Utility.RandomMinMax(8, 15)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("VortexBreath", "Void Vortex Breath", "Some creatures unleash a cone of withering breath.", (map, content) => AttachAttr(map, content, () => new XmlDVortexBreath(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(15, 40), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("EarthquakeStrike", "Earthquake Strike", "Some creatures unleash shockwaves on hit.", (map, content) => AttachAttr(map, content, () => new XmlEarthquakeStrike(Utility.RandomMinMax(10, 25), Utility.RandomMinMax(4, 8)), att => { att.Range = Utility.RandomMinMax(2, 4); if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Enrage", "Enrage", "Some creatures can fly into a rage, boosting melee damage temporarily.", (map, content) => AttachAttr(map, content, () => new XmlEnrage(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Flesheater", "Flesheater", "Some creatures feast on blood, causing bleed and healing themselves.", (map, content) => AttachAttr(map, content, () => new XmlFlesheater(Utility.RandomMinMax(45, 90), Utility.RandomMinMax(30, 60), 15, Utility.RandomMinMax(75, 125)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FreezeStrike", "Freeze Strike", "Some creatures freeze foes in place on hit.", (map, content) => AttachAttr(map, content, () => new XmlFreezeStrike(Utility.RandomMinMax(3, 6)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Frenzy", "Frenzy", "Some creatures erupt into a speed frenzy mid-fight.", (map, content) => AttachAttr(map, content, () => new XmlFrenzy(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("FrostStrike", "Frost Strike", "Some creatures inflict chilling cold damage over time.", (map, content) => AttachAttr(map, content, () => new XmlFrostStrike(Utility.RandomMinMax(2, 6), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GasCircle", "Gas Circle", "Some creatures unleash a ring of toxic gas on hit.", (map, content) => AttachAttr(map, content, () => new XmlGasCircle(Utility.RandomMinMax(8,12), Utility.RandomMinMax(10,25), 5, 2), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("GraspingStrike", "Grasping Strike", "Some creatures may paralyze foes with a crushing blow.", (map, content) => AttachAttr(map, content, () => new XmlGrasp(10.0, Utility.RandomMinMax(4, 8)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("LineStrike", "Line Strike", "Some creatures unleash a line of destructive force when attacking.", (map, content) => AttachAttr(map, content, () => new XmlLineAttack(Utility.RandomMinMax(2, 5), Utility.RandomMinMax(15, 30), Utility.RandomMinMax(6, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MagmaThrow", "Magma Throw", "Some creatures hurl molten magma during battle.", (map, content) => AttachAttr(map, content, () => new XmlMagmaThrow(Utility.RandomMinMax(8,12), Utility.RandomMinMax(15,30), Utility.RandomMinMax(6,10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ManaBurn", "Mana Burn", "Some creatures drain mana and convert it to damage.", (map, content) => AttachAttr(map, content, () => new XmlManaBurn(Utility.RandomMinMax(5, 15), Utility.RandomDouble() * 1.5 + 1.0, Utility.RandomMinMax(8, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("MushroomCircle", "Mushroom Circle", "Some creatures erupt with a ring of burning mushrooms.", (map, content) => AttachAttr(map, content, () => new XmlMushroomCircle(Utility.RandomMinMax(8, 14), Utility.RandomMinMax(10, 25), Utility.RandomMinMax(3, 6), Utility.RandomMinMax(2, 4)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("NaturesWrath", "Nature's Wrath", "Some creatures unleash nature’s fury on nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlNaturesWrath(Utility.RandomMinMax(5, 15), Utility.RandomMinMax(4, 8)), att => { att.Range = Utility.RandomMinMax(1, 3); if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("ParaBreath", "Paralyzing Breath", "Some creatures unleash a forward cone of fiery destruction.", (map, content) => AttachAttr(map, content, () => new XmlParaBreath(Utility.RandomMinMax(2, 5), Utility.RandomMinMax(20, 40), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PoisonAppleThrow", "Poison Apple Throw", "Occasionally, creatures throw poisoned apples that damage nearby enemies.", (map, content) => AttachAttr(map, content, () => new XmlPoisonAppleThrow(Utility.RandomMinMax(8, 12), 20, 8), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PoisonCloud", "Poison Cloud", "Creatures release a toxic cloud that poisons nearby enemies.", (map, content) => AttachAttr(map, content, () => new XmlPoisonCloud(Utility.RandomMinMax(1, 5), Utility.RandomMinMax(10, 20)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PoisonStrike", "Poison Strike", "Some creatures poison their attacker with a venomous strike.", (map, content) => AttachAttr(map, content, () => new XmlPoisonStrike(Poison.Regular, Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("PrismaticSpray", "Prismatic Spray", "Some creatures unleash a prismatic spray that damages and blinds foes.", (map, content) => AttachAttr(map, content, () => new XmlPrismaticSpray(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(10, 30), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("RuneBreath", "Rune Breath", "Occasionally, creatures unleash a rune attack, dealing damage over a range.", (map, content) => AttachAttr(map, content, () => new XmlRuneBreath(Utility.RandomMinMax(1,3), Utility.RandomMinMax(20,40), Utility.RandomMinMax(5,10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SawBreath", "Saw Breath", "Monsters unleash a saw cone that damages enemies.", (map, content) => AttachAttr(map, content, () => new XmlSawBreath(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(20, 50), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SilenceStrike", "Silence Strike", "Some creatures silence their foes on weapon strike.", (map, content) => AttachAttr(map, content, () => new XmlSilenceStrike(Utility.RandomMinMax(3, 7), Utility.RandomMinMax(8, 12)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SmokeCircle", "Smoke Circle", "Creatures unleash a fiery smoke circle that damages nearby enemies.", (map, content) => AttachAttr(map, content, () => new XmlSmokeCircle(Utility.RandomMinMax(8, 12), Utility.RandomMinMax(15, 25), Utility.RandomMinMax(3, 6), Utility.RandomMinMax(3, 5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SpikeCircle", "Spike Circle", "A circle of spikes erupts around the creature, dealing damage.", (map, content) => AttachAttr(map, content, () => new XmlSpikeCircle(Utility.RandomMinMax(8, 15), 20, 5, 4), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Sting", "Sting", "Some creatures' attacks inflict extra damage to poisoned targets.", (map, content) => AttachAttr(map, content, () => new XmlSting(Utility.RandomMinMax(15, 25)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("SummonStrike", "Summon Strike", "Some creatures have a chance to summon minions upon striking.", (map, content) => AttachAttr(map, content, () => new XmlSummonStrike("Drake", 5, 5, 30), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("TheftStrike", "Theft Strike", "Some creatures steal items from their victims with each strike.", (map, content) => AttachAttr(map, content, () => new XmlTheftStrike(Utility.RandomMinMax(8, 15)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("VortexBreath", "Vortex Breath", "A powerful vortex attack that damages enemies in a cone.", (map, content) => AttachAttr(map, content, () => new XmlVortexBreath(Utility.RandomMinMax(1, 3), Utility.RandomMinMax(20, 40), Utility.RandomMinMax(5, 10)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WaterBreath", "Water Breath", "Monsters release a damaging breath attack in front of them.", (map, content) => AttachAttr(map, content, () => new XmlWaterBreath(Utility.RandomMinMax(1, 5), 30, 10), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WeakenAura", "Weaken Aura", "Creatures occasionally weaken their foes, reducing Strength, Dexterity, and Intelligence.", (map, content) => AttachAttr(map, content, () => new XmlWeaken(), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("WebCooldown", "Web Cooldown", "Some creatures have a chance to paralyze their enemies in a frozen web.", (map, content) => AttachAttr(map, content, () => new XmlWebCooldown(Utility.RandomMinMax(3, 5)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			new MapModifier("Whirlwind", "Whirlwind", "A fierce whirlwind surrounds the creature, damaging nearby foes.", (map, content) => AttachAttr(map, content, () => new XmlWhirlwind(Utility.RandomMinMax(10, 30), Utility.RandomMinMax(3, 8)), att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
			
			//Treasure Moddfiers
			new MapModifier("BonusLoot", "Bonus Loot", "Some creatures carry extra treasure.", 
				(map, content) => AttachAttr(map, content, 
					() => new XmlExtraLoot("Gold", 150, 300, 1.0), 
					att => { if (Utility.RandomDouble() > 0.20) att.Delete(); })),
	
			#region → Commodity Loot Modifiers

			new MapModifier("IronIngot",           "Iron Ingot",           "Some creatures carry extra Iron Ingots.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("IronIngot",1,20,1.0), att => {})),
			new MapModifier("DullCopperIngot",     "Dull Copper Ingot",    "Some creatures carry extra Dull Copper Ingots.",    (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("DullCopperIngot",1,20,1.0), att => {})),
			new MapModifier("ShadowIronIngot",     "Shadow Iron Ingot",    "Some creatures carry extra Shadow Iron Ingots.",    (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("ShadowIronIngot",1,20,1.0), att => {})),
			new MapModifier("CopperIngot",         "Copper Ingot",         "Some creatures carry extra Copper Ingots.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("CopperIngot",1,20,1.0), att => {})),
			new MapModifier("BronzeIngot",         "Bronze Ingot",         "Some creatures carry extra Bronze Ingots.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("BronzeIngot",1,20,1.0), att => {})),
			new MapModifier("GoldIngot",           "Gold Ingot",           "Some creatures carry extra Gold Ingots.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("GoldIngot",1,20,1.0), att => {})),
			new MapModifier("AgapiteIngot",        "Agapite Ingot",        "Some creatures carry extra Agapite Ingots.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("AgapiteIngot",1,20,1.0), att => {})),
			new MapModifier("VeriteIngot",         "Verite Ingot",         "Some creatures carry extra Verite Ingots.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("VeriteIngot",1,20,1.0), att => {})),
			new MapModifier("ValoriteIngot",       "Valorite Ingot",       "Some creatures carry extra Valorite Ingots.",       (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("ValoriteIngot",1,20,1.0), att => {})),

			new MapModifier("Bacon",               "Bacon",                "Some creatures carry extra Bacon.",                (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Bacon",1,20,1.0), att => {})),
			new MapModifier("Ham",                 "Ham",                  "Some creatures carry extra Ham.",                   (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Ham",1,20,1.0), att => {})),
			new MapModifier("Sausage",             "Sausage",              "Some creatures carry extra Sausage.",               (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Sausage",1,20,1.0), att => {})),
			new MapModifier("RawChickenLeg",       "Raw Chicken Leg",      "Some creatures carry extra Raw Chicken Legs.",      (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RawChickenLeg",1,20,1.0), att => {})),
			new MapModifier("RawBird",             "Raw Bird",             "Some creatures carry extra Raw Birds.",             (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RawBird",1,20,1.0), att => {})),
			new MapModifier("RawLambLeg",          "Raw Lamb Leg",         "Some creatures carry extra Raw Lamb Legs.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RawLambLeg",1,20,1.0), att => {})),
			new MapModifier("RawRibs",             "Raw Ribs",             "Some creatures carry extra Raw Ribs.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RawRibs",1,20,1.0), att => {})),

			new MapModifier("Board",               "Board",                "Some creatures carry extra Boards.",                (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Board",1,20,1.0), att => {})),
			new MapModifier("BreadLoaf",           "Bread Loaf",           "Some creatures carry extra Bread Loaves.",          (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("BreadLoaf",1,20,1.0), att => {})),
			new MapModifier("ApplePie",            "Apple Pie",            "Some creatures carry extra Apple Pies.",            (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("ApplePie",1,20,1.0), att => {})),

			new MapModifier("StarSapphire",        "Star Sapphire",        "Some creatures carry extra Star Sapphires.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StarSapphire",1,20,1.0), att => {})),
			new MapModifier("Emerald",             "Emerald",              "Some creatures carry extra Emeralds.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Emerald",1,20,1.0), att => {})),
			new MapModifier("Sapphire",            "Sapphire",             "Some creatures carry extra Sapphires.",             (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Sapphire",1,20,1.0), att => {})),
			new MapModifier("Ruby",                "Ruby",                 "Some creatures carry extra Rubies.",                (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Ruby",1,20,1.0), att => {})),
			new MapModifier("Citrine",             "Citrine",              "Some creatures carry extra Citrines.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Citrine",1,20,1.0), att => {})),
			new MapModifier("Amethyst",            "Amethyst",             "Some creatures carry extra Amethysts.",             (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Amethyst",1,20,1.0), att => {})),
			new MapModifier("Tourmaline",          "Tourmaline",           "Some creatures carry extra Tourmalines.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Tourmaline",1,20,1.0), att => {})),
			new MapModifier("Amber",               "Amber",                "Some creatures carry extra Amber.",                 (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Amber",1,20,1.0), att => {})),
			new MapModifier("Diamond",             "Diamond",              "Some creatures carry extra Diamonds.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Diamond",1,20,1.0), att => {})),

			new MapModifier("BoltOfCloth",         "Bolt Of Cloth",        "Some creatures carry extra Bolts of Cloth.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("BoltOfCloth",1,20,1.0), att => {})),
			new MapModifier("Cotton",              "Cotton",               "Some creatures carry extra Cotton.",                (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Cotton",1,20,1.0), att => {})),
			new MapModifier("Wool",                "Wool",                 "Some creatures carry extra Wool.",                  (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Wool",1,20,1.0), att => {})),
			new MapModifier("Flax",                "Flax",                 "Some creatures carry extra Flax.",                  (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("Flax",1,20,1.0), att => {})),

			new MapModifier("OakLog",              "Oak Log",              "Some creatures carry extra Oak Logs.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("OakLog",1,20,1.0), att => {})),
			new MapModifier("AshLog",              "Ash Log",              "Some creatures carry extra Ash Logs.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("AshLog",1,20,1.0), att => {})),
			new MapModifier("YewLog",              "Yew Log",              "Some creatures carry extra Yew Logs.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("YewLog",1,20,1.0), att => {})),

			new MapModifier("RandomMagicWeapon",   "Magic Weapon",         "Some creatures carry a Random Magic Weapon.",       (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RandomMagicWeapon",1,1,0.5), att => {})),
			new MapModifier("RandomMagicArmor",    "Magic Armor",          "Some creatures carry Random Magic Armor.",          (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RandomMagicArmor",1,1,0.5), att => {})),
			new MapModifier("RandomMagicClothing", "Magic Clothing",       "Some creatures carry Random Magic Clothing.",       (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RandomMagicClothing",1,1,0.5), att => {})),
			new MapModifier("RandomMagicJewelry",  "Magic Jewelry",        "Some creatures carry Random Magic Jewelry.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RandomMagicJewelry",1,1,0.5), att => {})),

			new MapModifier("MaxxiaScroll",        "Maxxia Scroll",        "Some creatures carry a Maxxia Scroll.",             (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("MaxxiaScroll",1,1,0.2), att => {})),
			new MapModifier("SkillOrb",            "Skill Orb",            "Some creatures carry a Skill Orb.",                 (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("SkillOrb",1,1,0.2), att => {})),
			new MapModifier("StatCapOrb",          "StatCap Orb",          "Some creatures carry a StatCap Orb.",               (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StatCapOrb",1,1,0.1), att => {})),

			new MapModifier("DecorativeLootbox",   "Decorative Lootbox",   "Some creatures carry a Decorative Lootbox.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("DecorativeLootbox",1,1,0.1), att => {})),
			new MapModifier("GearLootbox",         "Gear Lootbox",         "Some creatures carry a Gear Lootbox.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("GearLootbox",1,1,0.1), att => {})),
			new MapModifier("KingLootbox",         "King Lootbox",         "Some creatures carry a King Lootbox.",              (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("KingLootbox",1,1,0.1), att => {})),
			new MapModifier("MagicalLootbox",      "Magical Lootbox",      "Some creatures carry a Magical Lootbox.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("MagicalLootbox",1,1,0.1), att => {})),
			new MapModifier("WorldLootbox",        "World Lootbox",        "Some creatures carry a World Lootbox.",             (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("WorldLootbox",1,1,0.1), att => {})),
			new MapModifier("StartingJewelryBox",  "Starting Jewelry Box", "Some creatures carry a Starting Jewelry Box.",      (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingJewelryBox",1,1,0.1), att => {})),
			new MapModifier("StartingClothes",     "Starting Clothes",     "Some creatures carry Starting Clothes.",            (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingClothes",1,1,0.1), att => {})),
			new MapModifier("StartingMedicine",    "Starting Medicine",    "Some creatures carry Starting Medicine.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingMedicine",1,1,0.1), att => {})),
			new MapModifier("PersonalArmoire",     "Personal Armoire",     "Some creatures carry a Personal Armoire.",          (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("PersonalArmoire",1,1,0.1), att => {})),
			new MapModifier("StartingCrate",       "Starting Crate",       "Some creatures carry a Starting Crate.",            (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingCrate",1,1,0.1), att => {})),

			new MapModifier("TownCommodityBarrel","Town Commodity Barrel","Some creatures carry a Town Commodity Barrel.",     (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("TownCommodityBarrel",1,1,0.1), att => {})),
			new MapModifier("TownTreasureChest",  "Town Treasure Chest",  "Some creatures carry a Town Treasure Chest.",       (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("TownTreasureChest",1,1,0.1), att => {})),
			new MapModifier("AbandonedRefuseChest","Abandoned Refuse Chest","Some creatures carry an Abandoned Refuse Chest.", (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("AbandonedRefuseChest",1,1,0.1), att => {})),
			new MapModifier("BossTreasureBox",    "Boss Treasure Box",    "Some creatures carry a Boss Treasure Box.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("BossTreasureBox",1,1,0.1), att => {})),
			new MapModifier("MurderTreasureChest","Murder Treasure Chest","Some creatures carry a Murder Treasure Chest.",    (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("MurderTreasureChest",1,1,0.1), att => {})),
			new MapModifier("RefuseOfTheFallen",  "Refuse Of The Fallen", "Some creatures carry the Refuse Of The Fallen.",   (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("RefuseOfTheFallen",1,1,0.1), att => {})),
			new MapModifier("TradeRouteChest",     "Trade Route Chest",    "Some creatures carry a Trade Route Chest.",        (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("TradeRouteChest",1,1,0.1), att => {})),
			new MapModifier("StartingGarbage",     "Starting Garbage",     "Some creatures carry Starting Garbage.",           (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingGarbage",1,1,0.1), att => {})),
			new MapModifier("StartingKitchen",     "Starting Kitchen",     "Some creatures carry a Starting Kitchen.",         (map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingKitchen",1,1,0.1), att => {})),
			new MapModifier("StartingTreasureChest","Starting Treasure Chest","Some creatures carry a Starting Treasure Chest.",(map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingTreasureChest",1,1,0.1), att => {})),
			new MapModifier("ElderLootbox","Elder Lootbox","Some creatures carry a Elder Lootbox.",(map, content) => AttachAttr(map, content, () => new XmlExtraLoot("StartingTreasureChest",1,1,0.1), att => {}))

			#endregion















































			
			
        };
    }

    #endregion

    public abstract class MagicMapBase : Item
    {
        // ── Random‐tier infra ─────────────────────────────────────
        private static readonly int[] _cumTierWeights;
		static MagicMapBase()
		{
			_cumTierWeights = new int[16];
			int sum = 0;
			for (int i = 1; i <= 16; i++)
			{
				int weight = (int)Math.Pow(2, 16 - i); // Exponential drop-off
				sum += weight;
				_cumTierWeights[i - 1] = sum;
			}
		}


        private static int PickRandomTier()
        {
            int roll = Utility.Random(_cumTierWeights[15]) + 1;
            for (int i = 0; i < 16; i++)
                if (roll <= _cumTierWeights[i])
                    return i + 1;
            return 16;
        }

        // ── Stored fields ───────────────────────────────────────────
        private int      _tier, _spawnRadius, _maxChests, _chestLevel;
        private TimeSpan _expiration;
        private List<string> _activeModIds = new List<string>();

        // ── Editable props ──────────────────────────────────────────
        public int Tier
        {
            get => _tier;
            set => _tier = Math.Max(1, Math.Min(16, value));
        }
        public int SpawnRadius
        {
            get => _spawnRadius;
            set => _spawnRadius = Math.Max(10, Math.Min(200, value));
        }
        public int MaxChests
        {
            get => _maxChests;
            set => _maxChests = Math.Max(1, Math.Min(10, value));
        }
        public int ChestLevel
        {
            get => _chestLevel;
            set => _chestLevel = Math.Max(1, Math.Min(6, value));
        }
        public TimeSpan ExpirationTime
        {
            get => _expiration;
            set => _expiration = value < TimeSpan.FromMinutes(0.5)
                                      ? TimeSpan.FromMinutes(0.5)
                                      : value;
        }
		private int _maxMonsters = 10;

		public virtual int MaxMonsters
		{
			get => _maxMonsters;
			set => _maxMonsters = Math.Max(1, Math.Min(value, 100)); // Clamp 1–100
		}
        public virtual int PortalHue    => 1174;
        public virtual int PortalSound  => 0x20E;

		// ── Monster-combo support ────────────────────────────────────────────────
		private Type[] _currentMonsterTypes;

		public virtual List<Type[]> MonsterTypeCombinations => new List<Type[]>
		{
			new[] { typeof(Lich), typeof(Dragon), typeof(Balron) },
			new[] { typeof(Orc), typeof(Orc), typeof(Orc) },
			new[] { typeof(Orc), typeof(Orc), typeof(ZanaTheDimensionalCartographer) }
		};

		public virtual Type[] MonsterTypes =>
			new[] { typeof(Lich), typeof(Dragon), typeof(Balron) }; // Default fallback

		protected void SelectMonsterCombination()
		{
			var combos = MonsterTypeCombinations;
			if (combos != null && combos.Count > 0)
				_currentMonsterTypes = combos[Utility.Random(combos.Count)];
			else
				_currentMonsterTypes = MonsterTypes;
		}


        // ── Original virtual members ────────────────────────────────
		// ── Backing fields for our override logic ────────────────────────
		protected List<Point3D> _overridePredefinedLocations;
		protected Map           _overrideDestinationFacet;

		// ── Replace your old expression‐bodied props with these ─────────
		/// <summary>Default encounter centres (or override if set).</summary>
		public virtual List<Point3D> PredefinedLocations
		{
			get
			{
				if (_overridePredefinedLocations != null)
					return _overridePredefinedLocations;

				// your original defaults here:
				return new List<Point3D>
				{
					new Point3D(783, 319, 46),
					new Point3D(553, 332, -1),
					new Point3D(690, 3946, -5)
				};
			}
		}

		/// <summary>Which facet to teleport into (or override if set).</summary>
		public virtual Map DestinationFacet
		{
			get
			{
				return _overrideDestinationFacet 
					   ?? Map.Felucca;   // or whatever your original default was
			}
		}



        /// <summary>Default spawn: monsters + chests + misc.</summary>
        protected virtual void SpawnChallenges(Point3D centre, Map map, SpawnedContent content, Mobile owner)
        {
            // your original code here...
            for (int i = 0; i < MaxMonsters; i++)
                SpawnMonster(centre, map, content);

            for (int i = 0; i < MaxChests; i++)
                SpawnChest(centre, map, content, owner);

            SpawnMiscObjects(centre, map, content);
        }

        // ── Modifier support ────────────────────────────────────────
        private enum Rarity { Common, Uncommon, Rare }
        public IEnumerable<MapModifier> ActiveModifiers
            => MapModifierRegistry.All.Where(m => _activeModIds.Contains(m.ID));

		private Rarity DetermineRarity()
		{
			int roll = Utility.Random(100); // returns 0 to 99

			if (roll < 80)
				return Rarity.Common;
			else if (roll < 95)
				return Rarity.Uncommon;
			else
				return Rarity.Rare;
		}


        public void RollModifiers()
        {
            _activeModIds.Clear();
            var pool = MapModifierRegistry.All;
            switch (DetermineRarity())
            {
                case Rarity.Common: break;
                case Rarity.Uncommon:
                    _activeModIds.AddRange(pool.OrderBy(_=>Utility.RandomDouble()).Take(Utility.RandomMinMax(1,2)).Select(m=>m.ID));
                    break;
                case Rarity.Rare:
                    _activeModIds.AddRange(pool.OrderBy(_=>Utility.RandomDouble()).Take(Utility.RandomMinMax(3, Math.Min(8, pool.Count))).Select(m=>m.ID));
                    break;
            }
        }

        // ── Constructor & defaults ─────────────────────────────────
        [Constructable]
        protected MagicMapBase(int itemID, string name, int hue) : base(itemID)
        {
            Name     = name;
            Hue      = hue;
            LootType = LootType.Regular;

            Tier           = PickRandomTier();
            SpawnRadius    = Utility.RandomMinMax(20, 120);
            MaxChests      = Utility.RandomMinMax(1, 10);
            ChestLevel     = Utility.RandomMinMax(1, 6);
            ExpirationTime = TimeSpan.FromMinutes(Utility.RandomMinMax(10, 40));

            RollModifiers();
        }

		public MagicMapBase(Serial serial) : base(serial)
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add($"Tier: {Tier}");
            list.Add($"Spawn Radius: {SpawnRadius}");
            list.Add($"Treasure Quantity: {MaxChests}");
            list.Add($"Treasure Rarity: {ChestLevel}");
            list.Add($"Expires In: {ExpirationTime.TotalMinutes:F1} min");

            foreach (var mod in ActiveModifiers)
                list.Add($"* {mod.Name}: {mod.Description}");
        }

        // ── Activation & teleport ───────────────────────────────────
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
                return;
            }

            SelectMonsterCombination();
            var origin      = from.Location;
            var originMap   = from.Map;
            var dest        = GetRandomLocation();
            var destMap     = DestinationFacet;

            var content = new SpawnedContent(this, from, origin, originMap, ExpirationTime);
            var pets    = GatherPets(from);
            TeleportPets(pets, dest, destMap);

            // base spawns
            SpawnChallenges(dest, destMap, content, from);

            // apply each modifier
            foreach (var m in ActiveModifiers)
                m.Apply(this, content);

            CreatePortals(origin, originMap, dest, destMap, content);
            from.SendMessage("The magic map crumbles as you are whisked away!");
            from.MoveToWorld(dest, destMap);
            Delete();
        }

        // ── (All your existing helpers: SpawnMonster, Chest, Misc, valid‐point, pet logic, portals…) ──
        // ── Default spawn helpers ───────────────────────────────────────────────

        protected virtual void SpawnMonster(Point3D centre, Map map, SpawnedContent content)
        {
            var pool = (_currentMonsterTypes != null && _currentMonsterTypes.Length > 0)
                       ? _currentMonsterTypes
                       : MonsterTypes;

            Type t = pool[Utility.Random(pool.Length)];
            var m = Activator.CreateInstance(t) as BaseCreature;
            if (m == null) return;

            var loc = GetValidMobileSpawnPoint(centre, map, SpawnRadius);
            m.MoveToWorld(loc, map);
            m.Home      = loc;
            m.RangeHome = SpawnRadius;
            content.SpawnedEntities.Add(m);
        }

		protected virtual void SpawnChest(Point3D centre, Map map, SpawnedContent content, Mobile from)
		{
			// Determine chest quality from level
			ChestQuality quality = ChestQuality.Standard;

			if (ChestLevel >= 5)
				quality = ChestQuality.Gold;
			else if (ChestLevel >= 3)
				quality = ChestQuality.Standard;
			else
				quality = ChestQuality.Rusty;

			// Create the chest
			var chest = new TreasureMapChest(from, ChestLevel, false)
			{
				ChestQuality = quality
			};

			// Hardcoded valid facets
			TreasureFacet[] validFacets = new TreasureFacet[]
			{
				TreasureFacet.Trammel,
				TreasureFacet.Felucca,
				TreasureFacet.Tokuno,
				TreasureFacet.TerMur,
				TreasureFacet.Eodon,
				TreasureFacet.Ilshenar,
				TreasureFacet.Malas
			};

			// Random facet from allowed list
			TreasureFacet facet = validFacets[Utility.Random(validFacets.Length)];

			// Resolve a compatible Map object from the facet
			Map resolvedMap = Map.Trammel;

			switch (facet)
			{
				case TreasureFacet.Trammel:
					resolvedMap = Map.Trammel;
					break;
				case TreasureFacet.Felucca:
					resolvedMap = Map.Felucca;
					break;
				case TreasureFacet.Tokuno:
					resolvedMap = Map.Tokuno;
					break;
				case TreasureFacet.TerMur:
				case TreasureFacet.Eodon: // Eodon is part of TerMur
					resolvedMap = Map.TerMur;
					break;
				case TreasureFacet.Ilshenar:
					resolvedMap = Map.Ilshenar;
					break;
				case TreasureFacet.Malas:
					resolvedMap = Map.Malas;
					break;
			}

			// Random package (Artisan, Mage, Warrior, etc.)
			var packages = Enum.GetValues(typeof(TreasurePackage)) as TreasurePackage[];
			TreasurePackage package = packages[Utility.Random(packages.Length)];

			// Create mock treasure map for loot generation
			var tmap = new TreasureMap
			{
				Level = ChestLevel,
				Package = package,
				TreasureLevel = (TreasureLevel)ChestLevel,
				Map = resolvedMap,
				Location = Point3D.Zero
			};

			chest.TreasureMap = tmap;

			// Place the chest in the world
			var pt = GetValidItemSpawnPoint(chest, centre, map, SpawnRadius);
			chest.MoveToWorld(pt, map);

			// Fill with loot
			TreasureMapInfo.Fill(from, chest, tmap);

			content.SpawnedEntities.Add(chest);
		}



        protected virtual void SpawnMiscObjects(Point3D centre, Map map, SpawnedContent content)
        {
            if (Utility.RandomDouble() < 0.30)
            {
                var ore = new ValoriteOre(5);
                var pt  = GetValidItemSpawnPoint(ore, centre, map, SpawnRadius);
                ore.MoveToWorld(pt, map);
                content.SpawnedEntities.Add(ore);
            }
        }

        // ── Helpers for valid spawn points ──────────────────────────────────────

        public virtual Point3D GetValidMobileSpawnPoint(Point3D centre, Map map, int radius)
        {
            for (int i = 0; i < 20; i++)
            {
                int x = centre.X + Utility.RandomMinMax(-radius, radius);
                int y = centre.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                var pt = new Point3D(x, y, z);
                if (map.CanSpawnMobile(pt))
                    return pt;
            }
            return centre;
        }

        public virtual Point3D GetValidItemSpawnPoint(Item item, Point3D centre, Map map, int radius)
        {
            int h = item.ItemData.Height;
            for (int i = 0; i < 20; i++)
            {
                int x = centre.X + Utility.RandomMinMax(-radius, radius);
                int y = centre.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                if (map.CanFit(x, y, z, h, false, false))
                    return new Point3D(x, y, z);
            }
            return centre;
        }

        // ── Pet gathering & teleport ───────────────────────────────────────────

        private List<BaseCreature> GatherPets(Mobile from)
        {
            var pets = new List<BaseCreature>();
            var playersMount = from.Mount as BaseCreature;

            foreach (var m in World.Mobiles.Values)
            {
                if (m is BaseCreature bc && bc != playersMount &&
                    ((bc.Controlled && bc.ControlMaster == from) ||
                     (bc.Summoned  && bc.SummonMaster == from)))
                {
                    pets.Add(bc);
                }
            }
            return pets;
        }

        private void TeleportPets(List<BaseCreature> pets, Point3D dest, Map map)
        {
            foreach (var pet in pets)
            {
                if (pet is IMount mount && mount.Rider != null && mount.Rider != pet.ControlMaster)
                    mount.Rider = null;
                pet.MoveToWorld(dest, map);
            }
        }

        // ── Random location & portals ──────────────────────────────────────────

        public    virtual Point3D GetRandomLocation()
        {
            if (PredefinedLocations.Count == 0)
                return Point3D.Zero;

            return PredefinedLocations[Utility.Random(PredefinedLocations.Count)];
        }

        protected virtual void CreatePortals(
            Point3D origin, Map originMap,
            Point3D destination, Map destinationMap,
            SpawnedContent content)
        {
            var op = new OriginPortal(PortalHue, PortalSound)
            {
                Destination    = destination,
                DestinationMap = destinationMap
            };
            op.MoveToWorld(origin, originMap);
            content.OriginPortal = op;

            var rp = new ReturnPortal(PortalHue, PortalSound)
            {
                Destination    = origin,
                DestinationMap = originMap
            };
            rp.MoveToWorld(destination, destinationMap);
            content.ReturnPortal = rp;

            Effects.SendLocationParticles(op, 0x3728, 10, 10, 2023);
            Effects.SendLocationParticles(rp, 0x3728, 10, 10, 2023);
            Effects.PlaySound(op.Location, originMap, PortalSound);
            Effects.PlaySound(rp.Location, destinationMap, PortalSound);
        }		
		


		#region → Modifier & Map‐tuning Utilities

		/// <summary>
		/// Instantly re‐rolls which MapModifiers are active (same as ChaosGlyph, but exposed).
		/// </summary>
		public void ShuffleModifiers()
		{
			RollModifiers();
		}

		/// <summary>
		/// Add a specific modifier by its registry ID (if it isn’t already applied).
		/// </summary>
		public void AddModifier(string modID)
		{
			if (!_activeModIds.Contains(modID)
			 && MapModifierRegistry.All.Any(m => m.ID == modID))
			{
				_activeModIds.Add(modID);
			}
		}

		/// <summary>
		/// Remove a specific modifier by its registry ID.
		/// </summary>
		public void RemoveModifier(string modID)
		{
			_activeModIds.Remove(modID);
		}

		/// <summary>
		/// Clear all currently‐active modifiers.
		/// </summary>
		public void ClearModifiers()
		{
			_activeModIds.Clear();
		}

		/// <summary>
		/// Remove one randomly‐chosen active modifier (if any).
		/// </summary>
		public void RemoveRandomModifier()
		{
			if (_activeModIds.Count == 0)
				return;

			var idx = Utility.Random(_activeModIds.Count);
			_activeModIds.RemoveAt(idx);
		}

		/// <summary>
		/// Add one randomly‐chosen modifier that isn’t already active (if any).
		/// </summary>
		public void AddRandomModifier()
		{
			// Get IDs of all mods not yet active
			var available = MapModifierRegistry.All
							  .Select(m => m.ID)
							  .Except(_activeModIds)
							  .ToArray();

			if (available.Length == 0)
				return;

			var pick = available[Utility.Random(available.Length)];
			_activeModIds.Add(pick);
		}

		/// <summary>
		/// Change the map’s tier (clamped 1–16).
		/// </summary>
		public void SetTierDirect(int newTier)
		{
			Tier = newTier;
		}

		/// <summary>
		/// Adjust spawn radius by delta (can be negative; clamped to 10–200).
		/// </summary>
		public void AdjustSpawnRadius(int delta)
		{
			SpawnRadius = SpawnRadius + delta;
		}

		/// <summary>
		/// Adjust maximum monsters by delta (clamped to 1–100).
		/// </summary>
		public void AdjustMaxMonsters(int delta)
		{
			MaxMonsters = MaxMonsters + delta;
		}

		/// <summary>
		/// Adjust maximum chests by delta (clamped to 1–10).
		/// </summary>
		public void AdjustMaxChests(int delta)
		{
			MaxChests = MaxChests + delta;
		}

		/// <summary>
		/// Add extra time to the map’s expiration.
		/// </summary>
		public void ExtendExpiration(TimeSpan extra)
		{
			ExpirationTime = ExpirationTime + extra;
		}

		/// <summary>
		/// Immediately re‐pick which monster‐type combo will spawn.
		/// </summary>
		public void ShuffleMonsterCombo()
		{
			SelectMonsterCombination();
		}
		
		#endregion
		


		#region → Location & Facet Utilities

		/// <summary>
		/// Add a new spawn location to the predefined list.
		/// </summary>
		public void AddPredefinedLocation(Point3D pt)
		{
			var list = PredefinedLocations.ToList();
			list.Add(pt);
			_overridePredefinedLocations = list;
		}

		/// <summary>
		/// Remove a predefined spawn location (by exact match).
		/// </summary>
		public void RemovePredefinedLocation(Point3D pt)
		{
			var list = PredefinedLocations.ToList();
			list.RemoveAll(p => p == pt);
			_overridePredefinedLocations = list;
		}

		/// <summary>
		/// Clear all predefined locations, so GetRandomLocation falls back to Zero.
		/// </summary>
		public void ClearPredefinedLocations()
		{
			_overridePredefinedLocations = new List<Point3D>();
		}

		/// <summary>
		/// Replace the entire predefined‐locations list.
		/// </summary>
		public void SetPredefinedLocations(IEnumerable<Point3D> pts)
		{
			_overridePredefinedLocations = pts.ToList();
		}


		/// <summary>
		/// Change the destination facet (e.g. Felucca &lt;–&gt; Trammel).
		/// </summary>
		public void SetDestinationFacet(Map newMap)
		{
			_overrideDestinationFacet = newMap;
		}

		#endregion



        // ── Serialization ─────────────────────────────────────────────
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(_tier);
            writer.Write(_spawnRadius);
            writer.Write(_maxChests);
            writer.Write(_chestLevel);
            writer.Write((double)_expiration.TotalSeconds);
			writer.Write(_maxMonsters); // write max monsters

            // version 3 = includes modifiers
            writer.Write(3);

            writer.Write(_activeModIds.Count);
            foreach (var id in _activeModIds)
                writer.Write(id);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            _tier        = reader.ReadInt();
            _spawnRadius = reader.ReadInt();
            _maxChests   = reader.ReadInt();
            _chestLevel  = reader.ReadInt();
            _expiration  = TimeSpan.FromSeconds(reader.ReadDouble());
			_maxMonsters = reader.ReadInt(); // ← add this

            int version = reader.ReadInt();
            if (version >= 3)
            {
                int count = reader.ReadInt();
                _activeModIds = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    var id = reader.ReadString();
                    if (MapModifierRegistry.All.Any(m => m.ID == id))
                        _activeModIds.Add(id);
                }
            }
            else
            {
                // legacy: just roll fresh
                RollModifiers();
            }
        }

        // ── SpawnedContent inner class (unchanged) ─────────────────────────
        public class SpawnedContent
        {
            public MagicMapBase   MapItem        { get; }
            public Mobile         Owner          { get; }
            public Point3D        OriginLocation { get; }
            public Map            OriginMap      { get; }

			public List<ISpawnable> SpawnedEntities { get; } = new List<ISpawnable>();
            public OriginPortal     OriginPortal    { get; set; }
            public ReturnPortal     ReturnPortal    { get; set; }
            public Timer            ExpirationTimer { get; }

            public SpawnedContent(MagicMapBase mapItem, Mobile owner,
                                  Point3D originLoc, Map originMap,
                                  TimeSpan duration)
            {
                MapItem        = mapItem;
                Owner          = owner;
                OriginLocation = originLoc;
                OriginMap      = originMap;

                // 5-minute warning
                Timer.DelayCall(duration - TimeSpan.FromMinutes(5), () =>
                {
                    foreach (var c in SpawnedEntities.OfType<BaseCreature>())
                        c.Say("The magical energies are weakening!");
                });

                // final expiration
                ExpirationTimer = Timer.DelayCall(duration, DeleteAllContent);
            }

            public void DeleteAllContent()
            {
                // If player's still in instance, bring them—and pets—home
                if (Owner != null && ReturnPortal != null && Owner.Map == ReturnPortal.Map)
                {
                    Owner.SendMessage("The magical map's power fades... whisking you and your companions back home!");

                    Effects.SendLocationParticles(
                        EffectItem.Create(Owner.Location, Owner.Map, EffectItem.DefaultDuration),
                        0x3728, 10, 10, 2023);
                    Effects.PlaySound(Owner.Location, Owner.Map, 0x1FE);

                    // teleport pets
                    var pets = new List<BaseCreature>();
                    var playersMount = Owner.Mount as BaseCreature;
                    foreach (var m in World.Mobiles.Values)
                    {
                        if (m is BaseCreature bc && bc != playersMount &&
                           ((bc.Controlled && bc.ControlMaster == Owner) ||
                            (bc.Summoned  && bc.SummonMaster == Owner)))
                        {
                            pets.Add(bc);
                        }
                    }
                    foreach (var pet in pets)
                    {
                        if (pet is IMount mount && mount.Rider != null && mount.Rider != pet.ControlMaster)
                            mount.Rider = null;
                        pet.MoveToWorld(OriginLocation, OriginMap);
                    }

                    Owner.MoveToWorld(OriginLocation, OriginMap);
                    Effects.SendLocationParticles(
                        EffectItem.Create(OriginLocation, OriginMap, EffectItem.DefaultDuration),
                        0x3728, 10, 10, 5023);
                    Effects.PlaySound(OriginLocation, OriginMap, 0x1FE);
                }

                // delete everything else
                foreach (var c in SpawnedEntities.OfType<BaseCreature>())
                    c.Delete();
                foreach (var it in SpawnedEntities.OfType<Item>())
                    if (it.RootParent == null && it.Map != Map.Internal)
                        it.Delete();

                OriginPortal?.Delete();
                ReturnPortal?.Delete();
                ExpirationTimer.Stop();
            }
        }	
				
    }
}
