using System;
using System.Collections;
using System.Collections.Generic; // Needed for lists
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network; // Needed for effects
using Server.Spells; // Needed for spell helpers and effects

namespace Server.Mobiles
{
    // The name "ArcaneSlug" is used for the class as requested,
    // but the in-game name reflects its magical, aberrant nature.
    [CorpseName("a warped arcane carcass")]
    public class ArcaneSlug : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextManaDrainTime;
        private DateTime m_NextRiftWalkTime;
        private DateTime m_NextArcanePulseTime;
        private DateTime m_NextHowlTime;

        // Unique Hue - Example: 1266 is a vibrant, unnatural violet.
        public const int UniqueHue = 1266;

        [Constructable]
        public ArcaneSlug() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3)
        {
            Name = "a Malidor ArcaneSlug";
            Body = 0x97; // Example body value
            Hue = UniqueHue;

            SetStr(400, 480);
            SetDex(200, 250);
            SetInt(450, 550);

            SetHits(1100, 1400);
            SetStam(200, 250);
            SetMana(450, 550);

            SetDamage(20, 26);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 65, 75);
            SetResistance(ResistanceType.Energy, 75, 85);

            SetSkill(SkillName.EvalInt, 105.1, 120.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 100.1, 115.0);
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Anatomy, 85.0, 95.0);

            Fame = 17000;
            Karma = -17000;

            VirtualArmor = 70;
            ControlSlots = 5;

            // Initialize ability cooldowns staggered
            m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftWalkTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));

            // Thematic Pack Items
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(5, 10)));
            PackItem(new DaemonBone(Utility.RandomMinMax(1, 3)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (this.Map != null && this.Map != Map.Internal && m == this && !m.Deleted && m.Alive)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    int tileType = Utility.Random(3);
                    Item hazard = null;
                    Point3D location = oldLocation;

                    if (!Map.CanFit(location.X, location.Y, location.Z, 16, false, false))
                    {
                        location.Z = Map.GetAverageZ(location.X, location.Y);
                    }

                    if (Map.CanFit(location.X, location.Y, location.Z, 16, false, false))
                    {
                        switch (tileType)
                        {
                            case 0: // Mana Drain Tile
                                hazard = new ManaDrainTile();
                                break;
                            case 1: // Vortex Tile
                                hazard = new VortexTile();
                                break;
                            case 2: // Necromantic Flamestrike Tile
                                hazard = new NecromanticFlamestrikeTile();
                                break;
                        }

                        if (hazard != null)
                        {
                            hazard.Hue = this.Hue;
                            hazard.MoveToWorld(location, this.Map);
                            Effects.SendLocationParticles(EffectItem.Create(location, this.Map, EffectItem.DefaultDuration), 0x37CC, 10, 20, UniqueHue, 0, 500, 0);
                        }
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            Mobile combatant = Combatant as Mobile;
            if (combatant == null || Map == null || Map == Map.Internal || !Alive || Deleted)
                return;

            if (DateTime.UtcNow >= m_NextManaDrainTime && InRange(combatant.Location, 8))
            {
                ManaLeechAttack();
                m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            }
            else if (DateTime.UtcNow >= m_NextArcanePulseTime && InRange(combatant.Location, 6))
            {
                ArcanePulseAttack();
                m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            else if (DateTime.UtcNow >= m_NextHowlTime && InRange(combatant.Location, 10))
            {
                DebilitatingHowl();
                m_NextHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
            }
            else if (DateTime.UtcNow >= m_NextRiftWalkTime && !InRange(combatant.Location, 2) && InRange(combatant.Location, 12))
            {
                RiftWalk();
                m_NextRiftWalkTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 30));
            }
        }

        public void ManaLeechAttack()
        {
            Mobile target = Combatant as Mobile;
            if (target == null || Map == null || !Alive)
                return;

            if (CanBeHarmful(target, false))
            {
                this.Say("*Your magic is mine!*");
                PlaySound(0x1F8);

                Effects.SendMovingParticles(this, target, 0x36FE, 7, 0, false, false, UniqueHue, 0, 9501, 1, 0, EffectLayer.Head, 0x100);
                DoHarmful(target);
                int damage = Utility.RandomMinMax(25, 40);
                int manaDrained = Utility.RandomMinMax(30, 60);

                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100, 0);

                if (target.Mana >= manaDrained)
                {
                    target.Mana -= manaDrained;
                    this.Mana += manaDrained;
                    target.SendMessage(0x22, "Your magical energy is siphoned away!");
                    target.FixedParticles(0x374A, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
                else if (target.Mana > 0)
                {
                    this.Mana += target.Mana;
                    target.Mana = 0;
                    target.SendMessage(0x22, "Your remaining magical energy is drained!");
                    target.FixedParticles(0x374A, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        public void ArcanePulseAttack()
        {
            if (Map == null || !Alive) return;

            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5016, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100, 0);
                    target.FixedParticles(0x37CC, 10, 25, 5039, UniqueHue, 0, EffectLayer.Waist);
                }
            }
        }

        public void RiftWalk()
        {
            Mobile target = Combatant as Mobile;
            if (target == null || Map == null || !Alive)
                return;

            Point3D targetLoc = target.Location;
            Point3D startLoc = this.Location;
            Point3D endLoc = targetLoc;
            bool foundLoc = false;

            for (int i = 0; i < 10; ++i)
            {
                Point3D checkLoc = new Point3D(targetLoc.X + Utility.RandomMinMax(-1, 1), targetLoc.Y + Utility.RandomMinMax(-1, 1), targetLoc.Z);
                if (Map.CanFit(checkLoc.X, checkLoc.Y, checkLoc.Z, this.BodyValue, true, false) && InLOS(checkLoc))
                {
                    endLoc = checkLoc;
                    foundLoc = true;
                    break;
                }
            }

            if (!foundLoc)
            {
                for (int i = 0; i < 10; ++i)
                {
                    Point3D checkLoc = new Point3D(this.X + Utility.RandomMinMax(-4, 4), this.Y + Utility.RandomMinMax(-4, 4), this.Z);
                    if (Map.CanFit(checkLoc.X, checkLoc.Y, checkLoc.Z, this.BodyValue, true, false) && InLOS(checkLoc))
                    {
                        endLoc = checkLoc;
                        foundLoc = true;
                        break;
                    }
                }
            }

            if (foundLoc)
            {
                Effects.SendLocationParticles(EffectItem.Create(startLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0);
                Effects.PlaySound(startLoc, Map, 0x1FE);

                this.Location = endLoc;
                this.ProcessDelta();

                Effects.SendLocationParticles(EffectItem.Create(endLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5023, 0);
                Effects.PlaySound(endLoc, Map, 0x1FE);

                IPooledEnumerable eable = Map.GetMobilesInRange(endLoc, 2);
                foreach (Mobile pm in eable)
                {
                    if (pm is PlayerMobile && pm.Alive && CanBeHarmful(pm, false))
                    {
                        pm.SendMessage(0x35, "The creature's sudden appearance disorients you!");
                    }
                }
                eable.Free();
            }
        }

        public void DebilitatingHowl()
        {
            if (Map == null || !Alive) return;

            this.Say("*Wails of the warped weave!*");
            PlaySound(GetAngerSound());
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    target.SendMessage(0x25, "An unnatural howl weakens your magical defenses!");
                    target.FixedParticles(0x374A, 10, 15, 5028, UniqueHue, 0, EffectLayer.Waist);

                    int reduction = -20; // Reduce Energy resistance by 20 points as a proxy for magic resist debuff
                    TimeSpan duration = TimeSpan.FromSeconds(10);
                    ResistanceMod energyMod = new ResistanceMod(ResistanceType.Energy, reduction);
                    target.AddResistanceMod(energyMod);

                    Timer.DelayCall(duration, () =>
                    {
                        if (target != null && !target.Deleted)
                            target.RemoveResistanceMod(energyMod);
                    });
                }
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(this.Location, this.Map, 0x207);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            int tilesToDrop = Utility.RandomMinMax(8, 12);
            List<Point3D> tileLocations = new List<Point3D>();

            for (int i = 0; i < tilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D tileLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(tileLoc.X, tileLoc.Y, tileLoc.Z, 16, false, false))
                {
                    tileLoc.Z = Map.GetAverageZ(tileLoc.X, tileLoc.Y);
                    if (!Map.CanFit(tileLoc.X, tileLoc.Y, tileLoc.Z, 16, false, false))
                        continue;
                }

                tileLocations.Add(tileLoc);
            }

            foreach (Point3D loc in tileLocations)
            {
                Item hazard = null;
                if (Utility.RandomBool())
                {
                    hazard = new ManaDrainTile();
                }
                else
                {
                    hazard = new VortexTile();
                }

                if (hazard != null)
                {
                    hazard.Hue = this.Hue;
                    hazard.MoveToWorld(loc, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration), 0x37CC, 10, 20, UniqueHue, 0, 500, 0);
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override double DispelDifficulty { get { return 130.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 7));

            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new RiftEssence(Utility.RandomMinMax(1, 3)));
            }
            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new MalidorsTwistedSash());
            }
        }

        public override int GetAngerSound() { return 0x4DE; }
        public override int GetIdleSound() { return 0x4DD; }
        public override int GetAttackSound() { return Utility.RandomBool() ? 0x4DC : 0x4E1; }
        public override int GetHurtSound() { return 0x4DF; }
        public override int GetDeathSound() { return 0x4DB; }

        public ArcaneSlug(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextManaDrainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftWalkTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextArcanePulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));

            this.Hue = UniqueHue;
        }
    }

    // --- Custom Loot Items ---
    public class RiftEssence : Item
    {
        [Constructable]
        public RiftEssence() : this(1) { }

        [Constructable]
        public RiftEssence(int amount) : base(0x26B4) // Example ItemID
        {
            Name = "Rift Essence";
            Hue = ArcaneSlug.UniqueHue;
            Stackable = true;
            Amount = amount;
            Weight = 0.1;
        }
        public RiftEssence(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    public class MalidorsTwistedSash : BodySash
    {
        [Constructable]
        public MalidorsTwistedSash() : base()
        {
            Name = "Malidor's Twisted Sash";
            Hue = ArcaneSlug.UniqueHue;
            Attributes.SpellDamage = 5;
            Attributes.LowerManaCost = 4;
            Attributes.RegenMana = 1;
        }
        public MalidorsTwistedSash(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}


