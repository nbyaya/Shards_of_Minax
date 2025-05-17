using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an ore brigand corpse")]
    public class OreBrigand : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextMagnetTime;
        private DateTime m_NextQuicksandTime;
        private DateTime m_NextMagmaTime;
        private Point3D m_LastLocation;

        // Unique metallic hue (coppery)
        private const int UniqueHue = 2005;

        [Constructable]
        public OreBrigand()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Use same body/sound logic as HumanBrigand
            this.Race = Race.Human;
            if (this.Female = Utility.RandomBool())
            {
                this.Body = 401;
                this.Name = NameList.RandomName("female");
            }
            else
            {
                this.Body = 400;
                this.Name = NameList.RandomName("male");
            }

            this.Title = "the ore brigand";
            this.Hue = UniqueHue;

            // Stats
            this.SetStr(350, 450);
            this.SetDex(200, 250);
            this.SetInt(100, 150);

            this.SetHits(1200, 1500);
            this.SetStam(200, 250);
            this.SetMana(100, 150);

            this.SetDamage(20, 30);

            // Damage profile
            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Fire, 30);

            // Resistances
            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire,     40, 50);
            this.SetResistance(ResistanceType.Cold,     30, 40);
            this.SetResistance(ResistanceType.Poison,   40, 50);
            this.SetResistance(ResistanceType.Energy,   20, 30);

            // Skills
            this.SetSkill(SkillName.Tactics,       100.1, 120.0);
            this.SetSkill(SkillName.Wrestling,     100.1, 120.0);
            this.SetSkill(SkillName.MagicResist,    80.0, 100.0);
            this.SetSkill(SkillName.DetectHidden,   85.0, 100.0);

            this.Fame = 15000;
            this.Karma = -15000;

            this.VirtualArmor = 70;
            this.ControlSlots = 3;

            // Initialize cooldowns
            DateTime now = DateTime.UtcNow;
            m_NextMagnetTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextQuicksandTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMagmaTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Outfit & gear (as per HumanBrigand)
            this.AddItem(new Shirt(Utility.RandomNeutralHue()));
            switch (Utility.Random(4))
            {
                case 0: this.AddItem(new Sandals()); break;
                case 1: this.AddItem(new Shoes());   break;
                case 2: this.AddItem(new Boots());   break;
                case 3: this.AddItem(new ThighBoots()); break;
            }

            if (this.Female)
            {
                if (Utility.RandomBool())
                    this.AddItem(new Skirt(Utility.RandomNeutralHue()));
                else
                    this.AddItem(new Kilt(Utility.RandomNeutralHue()));
            }
            else
            {
                this.AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }

            // Hair & weapon
            this.HairItemID       = this.Race.RandomHair(this.Female);
            this.HairHue          = UniqueHue;
            this.FacialHairItemID = this.Race.RandomFacialHair(this.Female);

            Item weapon = Loot.RandomWeapon();
            this.AddItem(weapon);
            if (weapon.Layer == Layer.OneHanded && Utility.RandomBool())
                this.AddItem(Loot.RandomShield());

            // Ore loot
            PackItem(new IronOre(Utility.RandomMinMax(15, 25)));
            PackItem(new DullCopperOre(Utility.RandomMinMax(10, 20)));
            PackGold(100, 200);

            m_LastLocation = this.Location;
        }

        public OreBrigand(Serial serial) : base(serial) { }

        // Pulls targets closer, once per cooldown
        public void MagneticSurge()
        {
            if (Combatant is Mobile target && this.InRange(target, 8) && CanBeHarmful(target, false))
            {
                this.Say("*Feel the pull of the deep ores!*");
                this.PlaySound(0x5C3);

                // Compute one‐tile pull
                int dx = Math.Sign(this.X - target.X);
                int dy = Math.Sign(this.Y - target.Y);
                int nx = target.X + dx, ny = target.Y + dy;
                int nz = this.Map.GetAverageZ(nx, ny);

                if (this.Map.CanFit(nx, ny, nz, 16, false, false))
                {
                    target.MoveToWorld(new Point3D(nx, ny, nz), this.Map);
                }

                DoHarmful(target);
                // Drain stamina
                int stamDrain = Utility.RandomMinMax(30, 50);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.SendMessage(0x22, "Your legs feel heavy with ore-laden gravity!");
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                }
            }
        }

        // Summons a quicksand pit under the combatant
        public void SummonQuicksandPit()
        {
            if (Combatant is Mobile target && this.InRange(target, 10) && CanBeHarmful(target, false))
            {
                this.Say("*Sink beneath the mine's depths!*");
                this.PlaySound(0x384);
                var loc = target.Location;

                QuicksandTile pit = new QuicksandTile();
                pit.Hue = UniqueHue;
                pit.MoveToWorld(loc, this.Map);
            }
        }

        // Creates bursting magma around itself in an AoE
        public void MoltenCoreBlast()
        {
            this.Say("*Feel the magma's wrath!*");
            this.PlaySound(0x208);

            List<Mobile> victims = new List<Mobile>();
            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    victims.Add(m);
            }
            eable.Free();

            // Spawn lava tiles and deal damage
            for (int i = 0; i < 6; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                Point3D drop = new Point3D(this.X + xOff, this.Y + yOff, this.Z);

                if (!this.Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                    drop.Z = this.Map.GetAverageZ(drop.X, drop.Y);

                HotLavaTile lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(drop, this.Map);
            }

            foreach (var vic in victims)
            {
                DoHarmful(vic);
                int dmg = Utility.RandomMinMax(30, 50);
                AOS.Damage(vic, this, dmg, 50, 0, 0, 0, 50); // 50% Phys, 50% Fire
                vic.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Track movement (optional earth-chunk effect)
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                if (this.Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    VortexTile shard = new VortexTile();
                    shard.Hue = UniqueHue;
                    shard.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || !Alive || this.Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            if (now >= m_NextMagnetTime)
            {
                MagneticSurge();
                m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (now >= m_NextQuicksandTime)
            {
                SummonQuicksandPit();
                m_NextQuicksandTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            else if (now >= m_NextMagmaTime && this.InRange(this.Combatant.Location, 6))
            {
                MoltenCoreBlast();
                m_NextMagmaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        public override void OnDeath(Container c)
        {
            if (this.Map != null)
            {
                this.Say("*The mine claims you too...*");
                this.PlaySound(0x214);

                // Scatter landmines around the corpse
                int count = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < count; i++)
                {
                    int xOff = Utility.RandomMinMax(-4, 4);
                    int yOff = Utility.RandomMinMax(-4, 4);
                    Point3D loc = new Point3D(this.X + xOff, this.Y + yOff, this.Z);

                    if (!this.Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = this.Map.GetAverageZ(loc.X, loc.Y);

                    LandmineTile mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, this.Map);

                    Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration),
                                                  0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // Standard loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));
        }

        public override bool BleedImmune       { get { return true; } }
        public override int TreasureMapLevel   { get { return 5; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize timers on load
            DateTime now = DateTime.UtcNow;
            m_NextMagnetTime    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextQuicksandTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMagmaTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
