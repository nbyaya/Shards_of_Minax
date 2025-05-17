using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;     // Needed for spell effects
using Server.Network;    // Needed for effects

namespace Server.Mobiles
{
    [CorpseName("an arcane assassin corpse")]
    public class ArcaneAssassin : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextShadowStepTime;
        private DateTime m_NextUmbralBladeTime;
        private DateTime m_NextManaVoidTime;
        private Point3D m_LastLocation; // For shadow trail effect

        // Unique Hue - Example: 1171 is a shadowy purple/blue. Adjust as needed.
        public const int UniqueHue = 1171;

        [Constructable]
        public ArcaneAssassin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction/attack speed
        {
            Name = "Arcane Assassin";
            Title = "Malidor's Shadow"; // Thematic title
            Female = Utility.RandomBool();
            Race = Race.Human;
            Hue = Race.RandomSkinHue(); // Skin hue remains normal
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue(); // Hair hue remains normal
            Race.RandomFacialHair(this);

            // --- Apply Unique Hue to Gear ---
            AddItem(new ThighBoots(UniqueHue));
            AddItem(new FancyShirt(UniqueHue));
            AddItem(new StuddedMempo()); // Mempo left undyed for contrast
            AddItem(new JinBaori(UniqueHue)); // Dyed JinBaori

            Item item;

            item = new StuddedGloves();
            item.Hue = UniqueHue;
            AddItem(item);

            item = new LeatherNinjaPants();
            item.Hue = UniqueHue;
            AddItem(item);

            item = new LightPlateJingasa(); // Helm, using black as a contrasting color
            item.Hue = 1; // Black
            AddItem(item);

            // Weapon - Kama feels fitting for an arcane assassin
            item = new Kama();
            item.Hue = UniqueHue; // Dyed weapon
            AddItem(item);

            // --- Significantly Boosted Stats - Hybrid Assassin/Mage ---
            SetStr(550, 650);    // Strong melee presence
            SetDex(350, 450);    // Very high dexterity for speed and skills
            SetInt(400, 500);    // High intelligence for mana pool and abilities

            SetHits(1000, 1200); // Increased survivability
            SetStam(250, 300);   // Good stamina for skills/movement
            SetMana(350, 450);   // Substantial mana pool for abilities

            SetDamage(18, 24);   // Increased base damage

            // Mixed damage profile - Physical and Arcane (Energy)
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60); // Arcane damage focus

            // --- Adjusted Resistances - Well-rounded with Energy focus ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 75, 85);

            // --- Enhanced Skills - Blend of Assassin and Arcane ---
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 110.0, 125.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0); // Or MaceFighting if using Kama
            SetSkill(SkillName.Anatomy, 110.0, 125.0);
            SetSkill(SkillName.Ninjitsu, 100.0, 120.0);
            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Magery, 85.0, 100.0);
            SetSkill(SkillName.EvalInt, 85.0, 100.0);
            SetSkill(SkillName.Meditation, 80.0, 95.0);

            Fame = 18000;  // High fame
            Karma = -18000; // Very evil

            VirtualArmor = 75; // High passive defense

            // Initialize ability cooldowns
            m_NextShadowStepTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextUmbralBladeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextManaVoidTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;
        }

        public ArcaneAssassin(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }

        // --- Movement Effect: Shadow Trail ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Shadow Trail Effect: Leaves hazardous tiles behind as it moves
            if (this.Map != null && this.Map != Map.Internal && this.Alive && this.Location != oldLocation && Utility.RandomDouble() < 0.15)
            {
                Point3D trailLoc = oldLocation;

                if (Map.CanFit(trailLoc.X, trailLoc.Y, trailLoc.Z, 16, false, false))
                {
                    VortexTile shadowTrail = new VortexTile();
                    shadowTrail.Hue = UniqueHue;
                    shadowTrail.Name = "lingering shadow";
                    shadowTrail.MoveToWorld(trailLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(trailLoc.X, trailLoc.Y);
                    if (Map.CanFit(trailLoc.X, trailLoc.Y, validZ, 16, false, false))
                    {
                        VortexTile shadowTrail = new VortexTile();
                        shadowTrail.Hue = UniqueHue;
                        shadowTrail.Name = "lingering shadow";
                        shadowTrail.MoveToWorld(new Point3D(trailLoc.X, trailLoc.Y, validZ), this.Map);
                    }
                }
            }
            m_LastLocation = this.Location; // Update last known location
        }

        // --- Thinking Process for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Ability selection logic
            if (Combatant == null && !this.Warmode && !this.Hidden && Utility.RandomDouble() < 0.05)
            {
                TryToHide();
            }
            else if (Combatant != null && !this.CanSee(Combatant) && Utility.RandomDouble() < 0.10)
            {
                TryToHide();
            }

            if (DateTime.UtcNow >= m_NextShadowStepTime && this.GetDistanceToSqrt((Mobile)Combatant) > 2 && this.GetDistanceToSqrt((Mobile)Combatant) < 10 && IsEnemy((Mobile)Combatant))
            {
                DoShadowStepAttack();
                m_NextShadowStepTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextUmbralBladeTime && this.InRange(((Mobile)Combatant).Location, 2) && IsEnemy((Mobile)Combatant))
            {
                DoUmbralBladeAttack();
                m_NextUmbralBladeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            else if (DateTime.UtcNow >= m_NextManaVoidTime && this.InRange(((Mobile)Combatant).Location, 8) && IsEnemy((Mobile)Combatant))
            {
                DoManaVoidAttack();
                m_NextManaVoidTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void TryToHide()
        {
            if (!Hidden)
                UseSkill(SkillName.Hiding);
        }

        // --- Missing Method Definition for AutoAsyncTarget ---
        private void AutoAsyncTarget()
        {
            // Example implementation: simply try to perform another harmful action on the current combatant.
            if (Combatant != null && CanBeHarmful((Mobile)Combatant, false))
            {
                DoHarmful((Mobile)Combatant);
            }
        }

        // --- Unique Ability: Shadow Step (Teleport & Attack/Debuff) ---
        public void DoShadowStepAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Mobile target = (Mobile)Combatant;
            Point3D targetLoc = target.Location;
            Point3D startLoc = this.Location;

            // Attempt to find a spot behind the target
            Point3D landingSpot = Point3D.Zero;
            bool foundSpot = false;

            // Calculate direction vector
            int dx = targetLoc.X - startLoc.X;
            int dy = targetLoc.Y - startLoc.Y;

            // Normalize (simplified)
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                dx = Math.Sign(dx);
                dy = 0;
            }
            else
            {
                dx = 0;
                dy = Math.Sign(dy);
            }

            // Try landing spot directly behind based on approach vector
            Point3D potentialSpot = new Point3D(targetLoc.X - dx, targetLoc.Y - dy, targetLoc.Z);

            if (Map.CanFit(potentialSpot.X, potentialSpot.Y, potentialSpot.Z, 16, false, false))
            {
                landingSpot = potentialSpot;
                foundSpot = true;
            }
            else // Fallback: try adjacent spots
            {
                for (int i = -1; i <= 1 && !foundSpot; ++i)
                {
                    for (int j = -1; j <= 1 && !foundSpot; ++j)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        potentialSpot = new Point3D(targetLoc.X + i, targetLoc.Y + j, targetLoc.Z);
                        if (Map.CanFit(potentialSpot.X, potentialSpot.Y, potentialSpot.Z, 16, false, false))
                        {
                            landingSpot = potentialSpot;
                            foundSpot = true;
                        }
                    }
                }
            }

            if (!foundSpot)
                return; // Couldn't find a valid spot

            this.Say("*Flicker*");
            // Effect: Fade out at start location
            Effects.SendLocationParticles(EffectItem.Create(startLoc, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);
            this.PlaySound(0x201);

            // Move the assassin
            this.Location = landingSpot;
            this.ProcessDelta();
            // Effect: Fade in at landing spot
            Effects.SendLocationParticles(EffectItem.Create(landingSpot, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Perform an immediate attack or apply a debuff
            if (target != null && CanBeHarmful(target, false))
            {
                DoHarmful(target);
                this.Combatant = target;
                this.Warmode = true;

                // Option: Apply a short debuff and send message
                target.SendMessage(0x35, "You feel disoriented by the sudden attack!");
                target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);

                // Example Debuff: Lower Physical Resistance temporarily
                int resistReduction = 15;
                TimeSpan duration = TimeSpan.FromSeconds(5);
                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, -resistReduction);
                target.AddResistanceMod(mod);
                Timer.DelayCall(duration, () =>
                {
                    target.RemoveResistanceMod(mod);
                    target.SendMessage("Your senses return to normal.");
                });

                Timer.DelayCall(TimeSpan.FromSeconds(0.1), () =>
                {
                    if (Alive && target.Alive)
                        AutoAsyncTarget();
                });
            }
        }

        // --- Unique Ability: Umbral Blade (Enhanced Melee Strike) ---
        public void DoUmbralBladeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Shadows bleed!*");
            this.PlaySound(0x52B);
            this.Animate(AnimationType.Attack, Utility.RandomMinMax(0, 2));

            Mobile target = (Mobile)Combatant;

            if (target != null && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Calculate enhanced damage
                int damage = Utility.RandomMinMax(45, 65);
                // Deal mixed damage: 30% Physical, 70% Energy
                AOS.Damage(target, this, damage, 30, 0, 0, 0, 70);

                // Visual hit effect
                target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);

                // Chance to apply 'Arcane Bleed'
                if (Utility.RandomDouble() < 0.33)
                {
                    target.SendMessage(0x22, "An unnatural chill seeps into your wounds!");
                    // Example of delayed energy damage ticks
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
                    {
                        if (target.Alive)
                        {
                            AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 100);
                            target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                        }
                    });
                    Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                    {
                        if (target.Alive)
                        {
                            AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 0, 0, 0, 100);
                            target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                        }
                    });
                }
            }
            else if (target != null)
            {
                // Direct damage to non-mobile target (fallback)
                int damage = Utility.RandomMinMax(45, 65);
                AOS.Damage(target, this, damage, 30, 0, 0, 0, 70);
            }
        }

        // --- Unique Ability: Mana Void (Targeted AoE Mana Drain/Damage) ---
        public void DoManaVoidAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Your focus wavers!*");
            PlaySound(0x1F8);

            Mobile target = (Mobile)Combatant;
            Point3D center = target.Location;

            // Visual effect at the center of the AoE
            Effects.SendLocationParticles(EffectItem.Create(center, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 30, UniqueHue, 0, 5039, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(center, 4);

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
                foreach (Mobile targetMobile in targets)
                {
                    if (targetMobile != null)
                    {
                        DoHarmful(targetMobile);
                        int manaDrained = Utility.RandomMinMax(30, 50);
                        int damage = 0;

                        if (targetMobile.Mana >= manaDrained)
                        {
                            targetMobile.Mana -= manaDrained;
                            targetMobile.SendMessage(0x22, "The void leeches your magical energy!");
                            damage = manaDrained / 2;
                        }
                        else
                        {
                            damage = targetMobile.Mana / 2;
                            targetMobile.Mana = 0;
                            targetMobile.SendMessage(0x22, "The void drains the last of your focus!");
                            damage += Utility.RandomMinMax(10, 20);
                        }

                        if (damage > 0)
                        {
                            AOS.Damage(targetMobile, this, damage, 0, 0, 0, 0, 100);
                        }

                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    }
                }
            }
        }

        // --- Death Effect: Unraveling Shadow ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Shadows... consume...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 40, UniqueHue, 0, 5052, 0);

            // Spawn hazardous shadow patches around the corpse
            int hazardsToDrop = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                Point3D hazardLocation = Point3D.Zero;
                bool foundLoc = false;
                for (int attempts = 0; attempts < 10 && !foundLoc; ++attempts)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    if (xOffset == 0 && yOffset == 0)
                        continue;

                    Point3D potentialLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (Map.CanFit(potentialLoc.X, potentialLoc.Y, potentialLoc.Z, 16, false, false))
                    {
                        hazardLocation = potentialLoc;
                        foundLoc = true;
                    }
                    else
                    {
                        potentialLoc.Z = Map.GetAverageZ(potentialLoc.X, potentialLoc.Y);
                        if (Map.CanFit(potentialLoc.X, potentialLoc.Y, potentialLoc.Z, 16, false, false))
                        {
                            hazardLocation = potentialLoc;
                            foundLoc = true;
                        }
                    }
                }

                if (foundLoc)
                {
                    ManaDrainTile shadowPatch = new ManaDrainTile();
                    shadowPatch.Hue = UniqueHue;
                    shadowPatch.Name = "unraveling shadow";
                    shadowPatch.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 5, 15, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.HighScrolls);
            AddLoot(LootPack.Potions, Utility.RandomMinMax(1, 3));

            if (Utility.RandomDouble() < 0.10)
            {
                // Example thematic loot
                PackItem(new ArcaneGem(Utility.RandomMinMax(3, 5)));
            }

            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll());
            }

            if (Utility.RandomDouble() < 0.05)
            {
                switch (Utility.Random(3))
                {
                    case 0:
                        PackItem(new Fukiya());
                        break;
                    case 1:
                        PackItem(new Shuriken(Utility.RandomMinMax(5, 15)));
                        break;
                    case 2:
                        PackItem(new SmokeBomb(Utility.RandomMinMax(1, 3)));
                        break;
                }
            }
        }

        // --- Standard Properties ---
        public override bool BleedImmune { get { return false; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus { get { return 40.0; } }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-initialize timers on load/restart
            m_NextShadowStepTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextUmbralBladeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextManaVoidTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
